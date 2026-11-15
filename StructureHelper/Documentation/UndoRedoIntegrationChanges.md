# Undo/Redo Common Infrastructure

**Status:** Implemented in `StructureHelperCommon`  
**Scope:** New common types only. Existing production elements were not modified.

---

## 1. New common files

Added under:

```text
StructureHelperCommon/Infrastructures/History/
```

| File | Contents |
|---|---|
| `HistoryScope.cs` | `HistoryScope`, `HistoryScopeFilter`, `UndoRedoHistoryEntry` |
| `UndoRedoContracts.cs` | `UndoRedoState`, `UndoRedoEventArgs`, `IUndoTransaction`, `IUndoRedoService` |
| `ProjectSnapshots.cs` | `IProjectSnapshot`, `IProjectSnapshotFactory`, `IProjectSnapshotRestorer`, `ProjectSnapshot`, `ProjectSnapshotFactory`, `ProjectSnapshotRestorer`, `ProjectCloneStrategy` |
| `UndoRedoService.cs` | `UndoRedoService` implementation and internal transaction implementation |

No existing project file, model, view model, view or test was changed.

---

## 2. Implemented behavior

### Explicit commit only

`UndoRedoService` creates history entries only inside:

```text
IUndoTransaction.Commit()
```

`Dispose()` never commits. An uncommitted transaction is rolled back or discarded.

### Single global history

The service has one undo stack and one redo stack for the attached project.

`HistoryScope` is metadata on each entry:

- `Project`
- `Analysis`
- `MultipleAnalyses`

It does not create separate stacks and does not change undo order.

### Scope metadata

`BeginTransaction` accepts:

```text
name
scope
affectedAnalysisIds
```

Validation:

- `Project` — no analysis IDs;
- `Analysis` — exactly one ID;
- `MultipleAnalyses` — at least two distinct IDs.

### Snapshot cloning

`ProjectSnapshotFactory` uses:

```text
CloneMode.PreserveIdentity
CloneDepth.PreserveReference
```

The resulting project snapshot:

- has a different root object from the live project;
- preserves IDs;
- preserves shared references inside the project;
- has no reference to a live mutable object;
- excludes file name and dirty flag.

### Restore

`ProjectSnapshotRestorer` restores into the existing `IProject` instance:

- validates the project ID;
- clones the snapshot again;
- replaces `VisualAnalyses` content;
- leaves the target root ID unchanged;
- does not change the target file name or dirty flag.

### Filtered history

`GetHistory(HistoryScopeFilter)` filters the global stack without changing its order.

Filters:

- all;
- current analysis;
- analysis only;
- project only.

`HistoryScopeFilter.ProjectOnly` returns only `HistoryScope.Project` entries.

---

## 3. Existing-element changes required for integration

The following changes are **not implemented**. They are the integration checklist for the application layer.

### 3.1 Application composition

**`StructureHelper/Windows/MainWindow/AnalysesManagerViewModel.cs`**

Add:

- an `UndoRedoService` / controller owned by the main view model;
- reattach or recreate the service when `ProgramSetting.CurrentProject` changes;
- `Clear()` on new/open project;
- expose `Undo`, `Redo`, `CanUndo`, `CanRedo`, `History` to the view.

Do not make the service static.

### 3.2 File lifecycle

**`StructureHelper/Windows/MainWindow/Analyses/FileLogic.cs`**

Add:

- `MarkSaved()` after a successful save;
- `SetProject(newProject)` after new/open;
- `Clear()` when a project is closed;
- reject save while a transaction is active;
- do not commit any transaction during save/open;
- keep history on save;
- discard history on open/new.

Existing `IsActual` handling must remain the dirty-state source.

### 3.3 Project snapshot/restore

**`StructureHelperCommon/Models/Projects/ProjectUpdateStrategy.cs`**

No change is required for the new snapshot restorer. Do not reuse `ProjectUpdateStrategy` as the undo restore path unless it is extended to use:

```text
CloneMode.PreserveIdentity
CloneDepth.PreserveReference
```

Its current child cloning uses the default mode and is not suitable for identity-preserving restore.

**`StructureHelperCommon/Models/Projects/Project.cs` / `IProject.cs`**

No API change is required. The new `ProjectCloneStrategy` handles project cloning. If the project model gains new persistent collections later, `ProjectCloneStrategy` must be updated.

### 3.4 Cross-section and analysis editors

**`StructureHelper/Windows/MainWindow/CrossSections/CrossSectionViewModel.cs`**

The view model now accepts an optional `IUndoRedoService` and exposes `UndoCommand` / `RedoCommand` plus example transaction methods. The application host must still pass the project-level service into the constructor.

Wrap these operations in explicit transactions:

- add/remove primitive;
- move/resize primitive;
- edit primitive properties;
- apply template/case;
- add/remove material;
- add/remove force action;
- add/remove calculator;
- apply calculation result to the model.

Use `HistoryScope.Analysis` with the containing analysis ID.

On successful mutation:

```text
transaction.Commit();
```

On cancel or failure:

```text
transaction.Rollback();
```

### 3.5 Property edit sessions

**Material, primitive, force and calculator view models**

For inline editors:

- begin transaction on edit start;
- commit on Enter / focus loss after validation;
- rollback on Escape or validation failure.

Do not commit from property setters.

For modal editors:

- begin when the dialog opens;
- commit only on `OK`;
- rollback on `Cancel` or close.

Relevant areas include:

- `MaterialsViewModel`
- `PrimitiveViewModelLogic`
- `ActionsViewModel`
- `AnalysisViewModelLogic`
- `AnalysisViewModel`
- calculator input view models
- `SelectItemVM<T>` copy/remove commands

### 3.6 Analysis lifecycle

**Analysis list / analysis manager**

- add analysis: `HistoryScope.Project`;
- delete analysis: `HistoryScope.Project`;
- reorder analysis: `HistoryScope.Project`;
- edit analysis metadata: `HistoryScope.Analysis` with the analysis ID;
- copy/move data between analyses: `HistoryScope.MultipleAnalyses` with all affected IDs.

Deleting an analysis must not delete history entries. Global undo may restore it. Filtered history must hide entries whose analysis no longer exists.

### 3.7 Main window UI

**Main window XAML**

The cross-section view now has Undo/Redo toolbar buttons bound to `UndoCommand` and `RedoCommand`. They remain disabled until the host supplies an `IUndoRedoService`.

Add:

- `Edit` menu with Undo/Redo;
- `Ctrl+Z` / `Ctrl+Y` key bindings;
- history panel or history window;
- scope filter selector;
- `CanExecute` bindings.

Undo/redo must remain global. Do not add per-analysis undo stacks.

### 3.8 Dirty state and save

**`ProgramSetting` / `FileLogic` / save logic**

- commit sets `IsActual = false`;
- undo/redo set `IsActual = false`;
- save sets `IsActual = true` and calls `MarkSaved()`;
- new/open set `IsActual = true` and clear history;
- rollback restores the previous dirty state as far as possible.

### 3.9 Background calculations

**Calculator runners and progress callbacks**

If a calculation writes results into the document:

- marshal to the UI thread;
- begin a transaction;
- apply results;
- explicitly commit.

Transient result caches remain non-undoable.

### 3.10 Tests

Add tests to the existing test project for:

- common service behavior;
- scope validation;
- filtered history;
- project snapshot reference identity;
- restore into the same project;
- editor transaction boundaries;
- save/open history behavior;
- undo/redo command enablement.

The common infrastructure itself should be tested separately from WPF.

---

## 4. Integration rules that must not be violated

1. Do not call `Commit()` from a property setter.
2. Do not call `Commit()` from `Dispose()`.
3. Do not call `Commit()` from a save operation.
4. Do not call `Commit()` from a background task without a UI transaction.
5. Do not create a separate undo stack per analysis.
6. Do not use `HistoryScope` to reorder or partition undo behavior.
7. Do not restore a snapshot into a different project ID.
8. Do not let the history panel change the global undo position.

---

## 5. Current validation status

- `StructureHelperCommon` builds successfully with the new infrastructure.
- No existing production element was modified by this change.
- The full solution currently has unrelated pre-existing test compile errors in `HeadMaterialUpdateStrategyTests` around `ISelfCloneStrategy`; those are not caused by the new common files.
