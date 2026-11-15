# Undo/Redo Realization Specification

**Status:** Proposed  
**Scope:** StructureHelper desktop application and project document model  
**Change policy:** This document is a specification only. No existing production element is changed by this specification.

---

## 1. Goal

Provide project-level undo/redo for all persistent document changes in StructureHelper.

The implementation must:

1. Use an explicit transaction commit as the only operation that creates an undo history entry.
2. Never commit automatically from:
   - property setters;
   - binding updates;
   - dialog close;
   - scope disposal;
   - file save;
   - window navigation;
   - calculation completion;
   - undo/redo itself.
3. Restore a complete, independent document graph with preserved internal reference identity.
4. Preserve object IDs across undo/redo so that references remain stable.
5. Integrate with the existing project, analysis, cross-section, material, primitive, force-action, and calculator models.
6. Keep the existing model API unchanged where possible. New behavior is introduced through new services and adapters.

---

## 2. Core Design Decision

### 2.1 Snapshot-based history

The first implementation will use **project snapshots**, not inverse command objects.

Reasons:

- StructureHelper has many mutable object graphs: projects, analyses, cross-sections, materials, primitives, force actions, calculators and their input data.
- The model has shared references, for example:
  - a primitive references a head material;
  - a calculator input references repository primitives and force actions;
  - a host primitive references another primitive.
- A snapshot captures all changes without requiring a hand-written inverse operation for every property.
- The existing clone infrastructure already supports independent deep copies and reference-preserving copies.

### 2.2 One project-level history

Undo/redo is scoped to the **current project document**, not to a single window or a single cross-section.

The project is the undo root because:

- `IProject` contains `VisualAnalyses`;
- each analysis contains its own cross-section and calculators;
- cross-section repositories contain materials, primitives, force actions and calculators;
- users expect `Ctrl+Z` to undo the last document change regardless of which panel is focused.

### 2.3 Explicit commit only

`BeginTransaction` captures the state before the operation.  
`Commit` captures the state after the operation and creates exactly one history entry.

No other method, event, setter or scope exit may create a history entry.

### 2.4 One global stack with explicit scope metadata

Undo/redo remains a **single project-level stack**. Analyses do not receive independent undo/redo stacks.

Each transaction records its scope:

- `Project` — project-level changes such as adding, deleting or reordering analyses;
- `Analysis` — a change that affects exactly one analysis;
- `MultipleAnalyses` — a cross-analysis change, such as copying a material or applying a result between analyses.

The scope is **metadata for display and filtering**, not a separate history sequence. `Ctrl+Z` always operates on the global stack, regardless of the currently selected analysis.

This is intentional. Separate stacks would make undo depend on window focus, break cross-analysis ordering and make shared references unsafe.

### 2.5 Why separate analysis stacks are not used

Separate per-analysis stacks are rejected for the first implementation because:

- the user expects undo to reverse the last document change, not the last change in the focused panel;
- operations such as adding/deleting an analysis or copying data between analyses have no single owner;
- analyses can share materials, primitives and force actions, so analysis-scoped snapshots can break reference integrity;
- deleting an analysis would invalidate its history;
- dirty-state tracking and `CanUndo`/`CanRedo` would have to be coordinated across multiple stacks;
- `Ctrl+Z` behavior would depend on focus and selection, which is difficult to explain and test.

A filtered history view is supported instead. If a future requirement makes analyses independent documents, that is a separate document model, not a second undo stack inside the current project.


---

## 3. Proposed Namespaces and Types

The exact folder names may follow the project conventions, but the responsibilities must remain separated as follows.

### 3.1 Common infrastructure

Proposed namespace:

```text
StructureHelperCommon.Infrastructures.History
```

Proposed types:

```text
IUndoRedoService
IUndoTransaction
UndoRedoState
UndoRedoHistoryEntry
HistoryScope
HistoryScopeFilter
IProjectSnapshot
IProjectSnapshotFactory
IProjectSnapshotRestorer
UndoRedoEventArgs
```

### 3.2 Application/UI integration

Proposed namespace:

```text
StructureHelper.Windows.UndoRedo
```

Proposed types:

```text
UndoRedoController
UndoRedoCommands
UndoRedoHistoryViewModel
```

The controller binds the service to the main window, the current project and WPF commands.

---

## 4. Public Contracts

### 4.1 Transaction states

```text
Idle
Active
Committing
RollingBack
UndoRedoInProgress
Disposed
```

Only one transaction may be active at a time.

### 4.2 `IUndoTransaction`

```text
interface IUndoTransaction
{
    Guid Id { get; }
    string Name { get; }
    HistoryScope Scope { get; }
    IReadOnlyList<Guid> AffectedAnalysisIds { get; }
    bool IsCommitted { get; }

    void Commit();
    void Rollback();
}
```

Rules:

- `Commit` is the **only** method that creates history.
- `Commit` throws if the transaction is not active.
- `Commit` throws if called twice.
- `Rollback` restores the before-snapshot and creates no history entry.
- `Dispose` is optional, but if implemented:
  - it must **never** commit;
  - disposing an uncommitted transaction must roll it back or discard it;
  - disposing a committed transaction must do nothing.
- No finalizer, timer, task completion or event may commit a transaction.

`Scope` and `AffectedAnalysisIds` are captured when the transaction begins and are stored on the resulting history entry. They do not affect the order of the global history.

### 4.2.1 History scope

```text
enum HistoryScope
{
    Project,
    Analysis,
    MultipleAnalyses
}
```

Rules:

- `Project` — project-level change; `AffectedAnalysisIds` is empty.
- `Analysis` — exactly one analysis is affected; `AffectedAnalysisIds` contains one ID.
- `MultipleAnalyses` — two or more analyses are affected; IDs are listed explicitly.
- Adding or deleting an analysis is `Project` scope.
- Changing data inside one analysis, including its cross-section, materials, primitives, actions and calculators, is `Analysis` scope.
- Copying or moving data between analyses is `MultipleAnalyses` scope.
- If a shared object is used by several analyses and the operation changes it, classify the transaction as `MultipleAnalyses` and list all affected analyses. Do not guess based only on the currently selected analysis.

`BeginTransaction` must validate these rules and throw for an invalid scope/ID combination.


### 4.3 `IUndoRedoService`

```text
interface IUndoRedoService
{
    bool CanUndo { get; }
    bool CanRedo { get; }
    bool IsTransactionActive { get; }
    int UndoCount { get; }
    int RedoCount { get; }

    event EventHandler<UndoRedoEventArgs> StateChanged;
    event EventHandler<UndoRedoEventArgs> DocumentRestored;

    IUndoTransaction BeginTransaction(
        string name,
        HistoryScope scope = HistoryScope.Project,
        IReadOnlyCollection<Guid>? affectedAnalysisIds = null);

    IReadOnlyList<UndoRedoHistoryEntry> GetHistory(HistoryScopeFilter filter);
    void Undo();
    void Redo();
    void Clear();

    void MarkSaved();
}
```

`GetHistory` is read-only and returns copies or immutable views of entries. Mutating the returned list must not affect the undo service.

`HistoryScopeFilter` is described in section 4.5.

### 4.4 History entry

```text
class UndoRedoHistoryEntry
{
    Guid Id;
    string Name;
    DateTimeOffset CreatedAt;

    HistoryScope Scope;
    IReadOnlyList<Guid> AffectedAnalysisIds;

    IProjectSnapshot Before;
    IProjectSnapshot After;
}
```

`Before` and `After` are immutable snapshots owned by the history service. They must never be handed to callers that can mutate them.

`Scope` and `AffectedAnalysisIds` are immutable. They are used only by `GetHistory` and the history panel.

### 4.5 History scope filter

```text
class HistoryScopeFilter
{
    bool IncludeProjectScope { get; }
    Guid? AnalysisId { get; }
}
```

Common filters:

| Filter | Result |
|---|---|
| All | Every history entry |
| Current analysis | Project-scope entries plus entries whose `AffectedAnalysisIds` contains the current analysis |
| Analysis only | Entries whose `AffectedAnalysisIds` contains the current analysis, excluding project entries |
| Project only | Entries with `HistoryScope.Project` |

Filtering must be applied to the global stack in its existing order. It must not create a second stack or change undo/redo semantics.

If an analysis has been deleted, entries whose IDs refer to it remain in history but are hidden from the current-analysis filter. They remain reachable by global undo. The history entry itself must not be removed merely because the analysis no longer exists.

Deleting an analysis is a `Project` scope transaction. An `Analysis`-scope entry that refers to a deleted analysis must never be executed by a per-analysis filter command.


### 4.6 Snapshot contracts

```text
interface IProjectSnapshot
{
    Guid ProjectId { get; }
    IProject Document { get; }
}

interface IProjectSnapshotFactory
{
    IProjectSnapshot Create(IProject project);
}

interface IProjectSnapshotRestorer
{
    void RestoreInto(IProject target, IProjectSnapshot snapshot);
}
```

`IProjectSnapshot.Document` must be a full independent clone. It must not share any mutable document object with the live project.

---

## 5. Snapshot Semantics

### 5.1 Cloning mode

Snapshot creation must use:

```text
CloneMode.PreserveIdentity
CloneDepth.PreserveReference
```

Meaning:

- IDs are preserved so undo/redo restores the same logical objects.
- Internal references are preserved inside the snapshot.
- The snapshot is still a full independent copy.
- No reference to a live source object may remain in the snapshot.

This is the same reference-integrity behavior implemented by `CrossSectionRepositoryCloneStrategy` for `CloneDepth.PreserveReference`.

Scope metadata does not change snapshot content. Every transaction, including an `Analysis`-scope transaction, stores a complete project snapshot. This is required because analysis operations can affect shared objects and because global undo must restore a coherent project, not a partial analysis graph.

### 5.2 What is included

The snapshot must include all persistent document state:

- project-level visual analyses;
- analysis metadata: name, tags, comment, color;
- cross-section data;
- materials and helper materials;
- primitives, shapes, centres, visual properties and NDM elements;
- force actions and force tuples;
- calculators and their input data;
- construction stages and history data;
- value-diagram data;
- all IDs and internal object relationships.

### 5.3 What is excluded

The snapshot must exclude transient or environment-specific state:

- `FullFileName`;
- `IsActual` / dirty flag;
- window and panel state;
- selected item, selection rectangles and zoom;
- tree expansion state;
- progress windows;
- calculation result caches that are not part of the saved document;
- trace loggers and log entries;
- delegates and event handlers;
- printer, export and clipboard state.

The exact set must match the project JSON persistence model. If a value is not saved to a `.shpj` file, it normally must not be undoable.

### 5.4 Snapshot correctness requirements

A snapshot must satisfy:

1. No object in the snapshot is the same instance as an object in the live project.
2. Shared references in the live project remain shared in the snapshot.
3. Shared references in the snapshot point to snapshot objects only.
4. IDs are equal to the live project IDs at capture time.
5. The snapshot can be captured while a transaction is active.
6. Snapshot capture itself must not modify the live project.

### 5.5 Project clone adapter

`IProject` currently has no project-level clone strategy.

The implementation must add a new adapter, for example:

```text
ProjectSnapshotCloneStrategy : ICloneStrategy<IProject>
```

It must clone:

- project visual analyses;
- each visual analysis's analysis object;
- each analysis's version processor and current version content;
- cross-section repositories through `CrossSectionRepositoryCloneStrategy`;
- calculator input data through their clone strategies.

The adapter must use one shared `ICloningStrategy` instance for the complete snapshot so that shared references are preserved across the whole project.

---

## 6. Transaction Lifecycle

### 6.1 Begin

`BeginTransaction(name, scope, affectedAnalysisIds)`:

1. Throws if a transaction is already active.
2. Validates the scope and analysis IDs:
   - `Project` with no IDs;
   - `Analysis` with exactly one ID;
   - `MultipleAnalyses` with two or more distinct IDs.
3. Captures `before = snapshotFactory.Create(currentProject)`.
4. Creates `IUndoTransaction` in `Active` state.
5. Stores `Scope` and `AffectedAnalysisIds` on the transaction.
6. Raises `StateChanged`.
7. Does not modify history.

A failed snapshot capture must leave the service idle and must not mutate the project. The scope metadata is still part of the transaction even if the transaction is rolled back; a rolled-back transaction creates no history entry.

When the current operation changes shared data used by several analyses, the caller must use `MultipleAnalyses` and list all affected analyses. Scope metadata is descriptive, not inferred by the service from the graph.

### 6.2 Mutate

The caller performs the document changes:

- add/remove material;
- add/remove primitive;
- edit primitive properties;
- add/remove force action;
- edit force tuples;
- add/remove calculator;
- edit analysis;
- import a template;
- apply a calculation result to the model.

Mutation APIs must not call `Commit`.

### 6.3 Commit

`Commit()`:

1. Throws if the transaction is not active.
2. Captures `after = snapshotFactory.Create(currentProject)`.
3. Validates that both snapshots belong to the same project ID.
4. Creates a history entry with `Before`, `After`, `Scope` and `AffectedAnalysisIds`.
5. Pushes it onto the undo stack.
6. Clears the redo stack.
7. Marks the project dirty (`IsActual = false`).
8. Moves service to `Idle`.
9. Raises `StateChanged`.

If `after` cannot be captured, the transaction must remain active and the exception must be propagated. The caller can then retry or roll back.

### 6.4 Rollback

`Rollback()`:

1. Throws if not active.
2. Restores the `before` snapshot into the live project.
3. Discards the transaction.
4. Does not add a history entry.
5. Does not clear the redo stack.
6. Marks the project dirty according to whether the restore differs from the last saved state.
7. Moves service to `Idle`.

### 6.5 Undo

`Undo()`:

1. Throws if a transaction is active.
2. Throws or returns without change if the undo stack is empty.
3. Pops the newest undo entry.
4. Moves service to `UndoRedoInProgress`.
5. Restores `entry.Before`.
6. Pushes the entry onto the redo stack.
7. Sets dirty state.
8. Raises `DocumentRestored` and `StateChanged`.
9. Returns to `Idle`.

### 6.6 Redo

`Redo()`:

1. Throws if a transaction is active.
2. Pops the newest redo entry.
3. Restores `entry.After`.
4. Pushes it back onto the undo stack.
5. Sets dirty state.
6. Raises events.
7. Returns to `Idle`.

### 6.7 New commit after undo

A successful commit after one or more undos clears the redo stack. This is standard linear history behavior.

### 6.8 No nested transactions

`BeginTransaction` while active throws.

The UI must not create nested transactions. If a command calls another command, the outer command owns the only transaction.

Future savepoint support is out of scope for this version.

---

## 7. Restore Semantics

### 7.1 Restore in place

The preferred restore operation is in-place restoration of the current project:

```text
snapshotRestorer.RestoreInto(currentProject, snapshot)
```

Requirements:

- the current `IProject` instance remains the same object;
- the project root ID remains unchanged;
- child collections are replaced or updated with cloned content;
- all child IDs match the snapshot;
- internal shared references are restored;
- existing child objects must not remain referenced by the restored graph.

The existing `ProjectUpdateStrategy` is not sufficient as-is because it clones children with the default `NewEntity` mode. The specification requires a new restore strategy that uses `PreserveIdentity` and `PreserveReference`.

### 7.2 View-model refresh

After a successful restore:

- `DocumentRestored` is raised;
- the active analysis/cross-section view models refresh;
- material, primitive, action and calculator collections are rebuilt from the restored model;
- selection is cleared or clamped to valid items;
- open dialogs that edit removed objects are closed or cancelled;
- no new transaction is created by the refresh.

Transient view-model state is not part of undo history.

### 7.3 Fallback: document replacement

If in-place restoration cannot preserve bindings, the implementation may replace the current project with a fresh clone.

If replacement is used:

- the old project is removed from `ProgramSetting.Projects`;
- the new project is inserted at the same position where possible;
- `CurrentProject` is updated;
- `AnalysesLogic.Refresh()` is called;
- history is not cleared, because the same document is being restored, not opened.

In-place restoration is the primary design. Replacement is a fallback only.

---

## 8. Transaction Boundaries

Every persistent mutation must be performed inside a transaction and committed only after the mutation succeeds.

### 8.1 Required transaction table

| Operation | Scope | Begin | Commit |
|---|---|---|---|
| Add material | `Analysis` | Before repository mutation | After successful add |
| Edit material | `Analysis` | When edit session starts | OK / focus loss / Enter |
| Delete material | `Analysis` | Before delete | After successful delete |
| Add primitive | `Analysis` | Before add | After successful add |
| Move/resize primitive | `Analysis` | On drag start | On drop |
| Edit primitive properties | `Analysis` | When edit session starts | OK / focus loss / Enter |
| Delete primitive | `Analysis` | Before delete | After successful delete |
| Add force action | `Analysis` | Before add | After successful add |
| Edit force tuple | `Analysis` | When edit session starts | OK / focus loss / Enter |
| Delete force action | `Analysis` | Before delete | After successful delete |
| Add calculator | `Analysis` | Before add | After successful add |
| Edit calculator input | `Analysis` | When editor opens | OK |
| Delete calculator | `Analysis` | Before delete | After successful delete |
| Edit analysis metadata | `Analysis` | When editor opens | OK |
| Add/remove analysis | `Project` | Before operation | After success |
| Apply template/case | `Analysis` or `MultipleAnalyses` | Before template application | After successful application |
| Import file into model | `Analysis` or `MultipleAnalyses` | Before import | Only after successful import and validation |
| Apply calculation result to model | `Analysis` | Before applying | After successful application |

### 8.2 Non-transactional operations

The following must not create history entries:

- save;
- save as;
- open;
- new project;
- close project;
- export;
- print;
- report generation;
- calculation run that only updates transient results;
- selection changes;
- zoom and pan;
- tree expansion;
- window layout changes;
- undo;
- redo;
- background progress updates that do not mutate the document.

### 8.3 Dialog transactions

For modal dialogs:

1. Begin transaction when the dialog opens.
2. Dialog edits a temporary clone or the live model, according to the existing dialog architecture.
3. `OK` applies changes, then explicitly commits.
4. `Cancel` applies no changes and explicitly rolls back.
5. Closing the window with unsaved changes follows the same rule as Cancel.
6. Dialog disposal must never commit.

### 8.4 Inline property edits

Property setters are not transaction boundaries.

For inline editors:

1. Begin when the user starts editing.
2. Allow temporary invalid values only inside the transaction.
3. Commit on Enter or focus loss if validation succeeds.
4. Rollback on Escape, invalid input or focus loss with validation failure.

This prevents one history entry per keystroke and prevents accidental commits from data binding.

### 8.5 Drag operations

1. Begin on drag start.
2. Update the model during drag.
3. Commit on successful drop.
4. Rollback if the drag is cancelled or dropped outside a valid target.

### 8.6 Explicit commit code shape

The intended usage is:

```text
var transaction = undoRedo.BeginTransaction(
    "Add material",
    HistoryScope.Analysis,
    new[] { analysisId });
try
{
    repository.AddHeadMaterial(material);
    transaction.Commit();
}
catch
{
    transaction.Rollback();
    throw;
}
```

The important rule is the explicit `Commit()` call. A `using` block must not be used as a substitute for commit.

---

## 9. Dirty State and File Integration

### 9.1 Dirty flag

`IProject.IsActual` remains the existing dirty-state indicator:

- `IsActual == true` means saved/clean;
- `IsActual == false` means unsaved changes.

Rules:

- successful `Commit` sets `IsActual = false`;
- successful `Undo` sets `IsActual = false`;
- successful `Redo` sets `IsActual = false`;
- successful save sets `IsActual = true`;
- open/new project sets `IsActual = true`;
- rollback restores the previous dirty flag as closely as possible.

### 9.2 Save

`FileLogic.SaveFile` and `SaveFileAs`:

- must not begin a transaction;
- must not commit a transaction;
- must not clear undo history;
- must call `MarkSaved()` on the undo service after a successful save;
- must leave redo intact, because redo is still a valid document transition.

`MarkSaved` updates the baseline used by the dirty-state calculation. It does not create history.

### 9.3 Open and new project

When a project is opened, created or closed:

- clear undo history;
- clear redo history;
- cancel any active transaction;
- create a new undo service or reset the existing service for the new project;
- subscribe to the new project's identity;
- do not attempt to undo across documents.

This is required because snapshots from one project must never be restored into another project.

### 9.4 Save while transaction active

Saving during an active transaction is not allowed by the UI. The save command must be disabled or must reject the operation with a clear message.

---

## 10. UI Integration

### 10.1 Main window commands

Add an `Edit` menu with:

- Undo;
- Redo;
- optional history tooltip showing the next operation name.

Keyboard shortcuts:

- `Ctrl+Z` — Undo;
- `Ctrl+Y` — Redo;
- `Ctrl+Shift+Z` — Redo, optional.

Commands must use WPF `CanExecute`:

- Undo enabled only when `CanUndo` is true and no transaction is active;
- Redo enabled only when `CanRedo` is true and no transaction is active.

`Ctrl+Z` and `Ctrl+Y` always operate on the global project history. They must not be routed to a hidden per-analysis history.

### 10.2 Command descriptions

Transaction names are user-visible.

Use short, stable names:

- `Add material`
- `Edit material`
- `Delete primitive`
- `Move primitive`
- `Add force action`
- `Edit calculator`
- `Apply template`

Do not include timestamps or object display names unless they are stable.

The history panel may append the affected analysis name for display, but the transaction name itself is the stable command name.

### 10.3 Focus behavior

`Ctrl+Z` must work from:

- cross-section canvas;
- material editor;
- force editor;
- calculator editor;
- analysis tree.

Text boxes must keep their local text undo where appropriate. The application-level undo must only run when the focused control does not handle the command. This must be implemented through WPF command routing, not by intercepting all keystrokes.

### 10.4 History panel and scope filter

The application may expose a read-only history panel.

Default filter is **Current analysis**:

- project-scope entries are always shown;
- entries whose `AffectedAnalysisIds` contains the selected analysis are shown;
- entries for other analyses are hidden.

The panel may offer:

- All;
- Current analysis;
- Analysis only;
- Project only.

The panel must display the entry name, timestamp, scope and affected analysis names.

Selecting a history entry may show details, but it must not change the global undo position. The first version must not implement a command that skips unrelated entries to undo a per-analysis change; that would create ordering ambiguity.

A future optional command `Undo last change in this analysis` may be added only if it is implemented by scanning the global stack and clearly explaining that earlier unrelated changes remain. It is not part of this version.

### 10.5 Busy state

While `Undo`/`Redo` or snapshot restore is running:

- show a busy cursor;
- disable document commands;
- do not allow a second undo/redo;
- do not allow a transaction to begin.

---

## 11. Service Lifecycle and Ownership

The service must be owned by the application-level main view model, not by individual editors.

Recommended ownership:

```text
AnalysesManagerViewModel
    └── UndoRedoController
            └── IUndoRedoService
```

The controller may be created once and re-bound when `ProgramSetting.CurrentProject` changes.

The service must not be a static global. A static service makes tests and multiple open projects unsafe.

---

## 12. Concurrency and Threading

- All document mutations and undo/redo operations must run on the WPF UI thread.
- The service is not thread-safe.
- Background calculations may complete on worker threads, but any model mutation must be marshalled to the UI thread before beginning a transaction.
- Calculation results that are not written to the document are not undoable.
- If a background operation applies results to the model, it must:
  1. marshal to the UI thread;
  2. begin a transaction;
  3. apply results;
  4. explicitly commit;
  5. handle failure by rollback.

---

## 13. Memory and History Limits

Full project snapshots consume memory. The first version must:

- keep a configurable maximum, default `50` undo entries;
- discard the oldest entry when the limit is exceeded;
- keep the redo stack bounded by the same limit;
- never serialize history to the project file;
- never load history from a project file;
- release snapshots when history is cleared or the project is closed.

A future optimization may switch to command-based deltas or compressed snapshots. That is not part of this version.

---

## 14. Error Handling

### 14.1 Mutation failure

If the operation throws before commit:

- catch it at the command boundary;
- call `Rollback()`;
- rethrow or display the existing error message;
- do not create a history entry.

### 14.2 Commit failure

If snapshot capture or validation fails during `Commit`:

- keep the transaction active;
- do not change history;
- report the error;
- allow the caller to retry `Commit` or call `Rollback`.

### 14.3 Restore failure

If restoring a snapshot fails:

- do not pop the history entry permanently;
- keep the current document unchanged where possible;
- move the service to a faulted/idle state;
- raise an error event;
- do not allow redo until the issue is resolved.

### 14.4 Invalid API calls

The following must throw a clear `InvalidOperationException` or project-specific infrastructure exception:

- begin while active;
- commit without active transaction;
- rollback without active transaction;
- commit twice;
- undo while active;
- redo while active;
- begin during undo/redo.

---

## 15. Testing Specification

### 15.1 Service unit tests

1. Begin creates an active transaction.
2. Commit creates exactly one history entry.
3. No commit occurs when a transaction is abandoned or disposed.
4. Rollback restores the before state and creates no history.
5. Undo restores the before state and moves the entry to redo.
6. Redo restores the after state.
7. A new commit clears redo.
8. Undo/redo are disabled while a transaction is active.
9. Nested begin throws.
10. Commit twice throws.
11. History limit removes the oldest entry.
12. `StateChanged` is raised at the correct times.
13. `DocumentRestored` is raised after undo/redo.
14. `Analysis` scope requires exactly one analysis ID.
15. `MultipleAnalyses` scope requires at least two distinct IDs.
16. `Project` scope rejects analysis IDs.
17. History entry stores the scope and affected analysis IDs from `BeginTransaction`.
18. `GetHistory` filters by current analysis without changing undo/redo order.
19. A project-scope entry is visible in every analysis filter.
20. An analysis-scope entry is hidden when filtering by another analysis.

### 15.2 Snapshot tests

1. Snapshot is not the same object as the live project.
2. Nested analyses are not the same objects.
3. Shared material referenced by two primitives remains one shared object in the snapshot.
4. Calculator input references the snapshot's cloned primitive, not the live primitive.
5. IDs are preserved.
6. No live object is referenced from the snapshot.
7. Snapshot capture does not mutate the live project.

### 15.3 Integration tests

1. Add material, commit, undo, verify material removed.
2. Add material, commit, redo, verify material restored.
3. Add two primitives sharing one material, undo, redo, verify shared reference is maintained.
4. Edit a calculator input referencing a primitive, undo, verify primitive and input are coherent.
5. Cancel a dialog after changes, verify rollback and no history.
6. Commit a dialog, verify exactly one history entry.
7. Save does not clear undo history.
8. Open/new project clears history.
9. Closing a project with unsaved changes follows existing save prompt behavior.
10. Command `CanExecute` updates correctly.
11. Add an analysis, commit as `Project` scope, undo, verify it is removed.
12. Add an analysis, commit as `Project` scope, verify it appears in all analysis filters.
13. Edit an analysis, commit as `Analysis` scope, verify the entry appears only for that analysis.
14. Copy a material between two analyses, commit as `MultipleAnalyses` scope, verify both analysis IDs are recorded.
15. Delete an analysis that has older history entries, verify global undo still restores it and the filtered view hides the removed analysis.
16. Select a history entry in the panel, verify global undo position does not change.

### 15.4 WPF smoke tests

- Undo/Redo menu items enable and disable correctly.
- `Ctrl+Z` and `Ctrl+Y` work from the main document area.
- Text editor local undo is not broken.
- Restored document refreshes the analysis tree and canvas.
- Selection is cleared or valid after restore.

---

## 16. Acceptance Criteria

The implementation is accepted when:

1. Every persistent user edit is performed inside an explicit transaction.
2. No history entry exists without an explicit `Commit`.
3. Undo restores the exact previous document state.
4. Redo restores the exact next document state.
5. Snapshots are independent copies with internal reference identity preserved.
6. IDs remain stable across undo/redo.
7. Shared references inside the restored document remain shared.
8. No snapshot references a live source object.
9. Save does not create or clear history.
10. Open/new project clears history.
11. The project dirty flag updates correctly.
12. Every history entry has a valid scope and affected analysis IDs.
13. Global undo/redo order is independent of the selected analysis and active history filter.
14. The solution builds and all new tests pass.

---

## 17. Out of Scope

- Persistent undo history across application restarts.
- Collaborative or multi-user editing.
- Undo of transient calculation results.
- Undo of window layout, selection, zoom or tree expansion.
- Transaction savepoints and nested transactions.
- Command-based delta history.
- Undo across project files.
- Separate undo/redo stacks per analysis.
- Per-analysis history sequences or per-analysis `CanUndo`/`CanRedo`.
- Skipping unrelated history entries to undo a per-analysis change.
- Automatic commits from property setters, timers, background tasks or disposal.

---

## 18. Implementation Sequence

1. Add history contracts, scope contracts and snapshot contracts.
2. Add project snapshot clone strategy using `PreserveIdentity` and `PreserveReference`.
3. Add project snapshot restore strategy.
4. Implement `UndoRedoService` and scope validation/unit tests.
5. Add transaction integration to the main view-model controller.
6. Add scope metadata to every transaction boundary.
7. Wrap add/remove commands in explicit transactions.
8. Wrap dialog and inline-edit sessions in explicit transactions.
9. Add Undo/Redo UI commands and keyboard routing.
10. Add the filtered history panel.
11. Integrate save/open/new dirty-state and history reset behavior.
12. Add integration and WPF smoke tests.

No step in this sequence may introduce automatic commit behavior.
