using NUnit.Framework;
using StructureHelper.Windows.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace StructureHelperTests.UnitTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SaveableConstructorTests
    {
        [Test]
        public void EveryConcreteSaveable_ShouldHaveIdConstructorThatAssignsId()
        {
            var assemblies = new[]
            {
                typeof(ISaveable).Assembly,
                typeof(CrossSection).Assembly,
                typeof(DataAccess.DTOs.HeadMaterialDTO).Assembly,
                typeof(CustomConcreteMaterialView).Assembly
            }.Distinct();

            var failures = new List<string>();
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(x => x is not null).ToArray()!;
                }

                foreach (var type in types.Where(x =>
                             x is not null
                             && x.IsClass
                             && !x.IsAbstract
                             && !x.IsGenericTypeDefinition
                             && typeof(ISaveable).IsAssignableFrom(x)))
                {
                    var constructor = type!.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(x => x.GetParameters().Length == 1
                            && x.GetParameters()[0].ParameterType == typeof(Guid));
                    if (constructor is null)
                    {
                        failures.Add($"{type.FullName}: no public (Guid) constructor");
                        continue;
                    }

                    var id = Guid.NewGuid();
                    try
                    {
                        var instance = constructor.Invoke(new object[] { id });
                        var idProperty = type.GetProperty("Id");
                        if (idProperty is null)
                        {
                            failures.Add($"{type.FullName}: no Id property");
                            continue;
                        }

                        var actual = (Guid)idProperty.GetValue(instance)!;
                        if (actual != id)
                        {
                            failures.Add($"{type.FullName}: Id was not assigned");
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        failures.Add($"{type.FullName}: constructor threw {ex.InnerException?.GetType().Name}: {ex.InnerException?.Message}");
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{type.FullName}: {ex.GetType().Name}: {ex.Message}");
                    }
                }
            }

            Assert.That(failures, Is.Empty, string.Join(Environment.NewLine, failures));
        }
    }
}
