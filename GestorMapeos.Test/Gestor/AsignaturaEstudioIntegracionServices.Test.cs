using System;
using System.Collections.Generic;
using System.Linq;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.AsignaturaIntegracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class AsignaturaEstudioIntegracionServicesTest : GestorMapeosServicesTest
    {
        private AsignaturaIntegracion PrepararAsignaturaEstudioIntegracion()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();

            var resAsignaturaPlan = CREATE.CrearAsignaturaPlanIntegracionSaveChange(gestorContext, erpContext);
            var asignaturaPlanIntegracion = resAsignaturaPlan.Item1;
            var plantillaAsignatura = resAsignaturaPlan.Item2;
            var asignaturaUnir = CREATE.PrepararAsignaturaUnir();
            asignaturaUnir.PlantillasAsignaturas = new List<PlantillaAsignatura>
            {
                plantillaAsignatura
            };
            gestorContext.AsignaturaUnir.Add(asignaturaUnir);
            gestorContext.SaveChanges();

            var persisted = new AsignaturaIntegracion
            {
                Id = asignaturaUnir.Id,
                AsignaturaPlanIntegracionId = asignaturaPlanIntegracion.Id
            };

            return persisted;
        }

        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            // Act
            var result = target.Crear(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = asignaturaEstudioIntegracion.Id,
                IdAsignaturaPlan = asignaturaEstudioIntegracion.AsignaturaPlanIntegracionId
            });

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }

        [TestMethod]
        public void CrearTestCasoMalo1()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            // Act
            var result = target.Crear(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = random.Next(1, int.MaxValue),
                IdAsignaturaPlan = random.Next(1, int.MaxValue)
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void CrearTestCasoMalo2()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);
            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();

            var resAsignaturaPlanIntegracion = CREATE.CrearAsignaturaPlanIntegracionSaveChange(gestorContext, erpContext);
            var otraAsignaturaPlanIntegracion = resAsignaturaPlanIntegracion.Item1;

            // Act
            var result = target.Crear(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = asignaturaEstudioIntegracion.Id,
                IdAsignaturaPlan = otraAsignaturaPlanIntegracion.Id
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void CrearTestCasoMalo3()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);
            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            gestorContext.AsignaturaIntegracion.Add(asignaturaEstudioIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Crear(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = asignaturaEstudioIntegracion.Id,
                IdAsignaturaPlan = asignaturaEstudioIntegracion.AsignaturaPlanIntegracionId
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void ModificarTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            gestorContext.AsignaturaIntegracion.Add(asignaturaEstudioIntegracion);
            gestorContext.SaveChanges();

            var asignaturaPlanIntegracion = gestorContext.PlantillaAsignaturaIntegracion.Find(asignaturaEstudioIntegracion.AsignaturaPlanIntegracionId);

            var resAsignaturaPlanIntegracion = CREATE.CrearAsignaturaPlanIntegracionSaveChange(gestorContext, erpContext);
            var otraAsignaturaPlanIntegracion = gestorContext.PlantillaAsignaturaIntegracion.Find(resAsignaturaPlanIntegracion.Item1.Id);
            otraAsignaturaPlanIntegracion.PlantillaAsignatura = asignaturaPlanIntegracion.PlantillaAsignatura;
            gestorContext.SaveChanges();

            // Act
            var result = target.Modificar(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = asignaturaEstudioIntegracion.Id,
                IdAsignaturaPlan = otraAsignaturaPlanIntegracion.Id
            });

            var persisted = gestorContext.AsignaturaIntegracion.First(ae => ae.Id == asignaturaEstudioIntegracion.Id);
            var hasUpdated = persisted.AsignaturaPlanIntegracionId == otraAsignaturaPlanIntegracion.Id;

            // Assert
            Assert.IsTrue(result.HasErrors == false);
            Assert.IsTrue(hasUpdated);
        }

        [TestMethod]
        public void ModificarTestCasoMalo1()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            // Act
            var result = target.Modificar(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = random.Next(1, int.MaxValue),
                IdAsignaturaPlan = random.Next(1, int.MaxValue)
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void ModificarTestCasoMalo2()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            gestorContext.AsignaturaIntegracion.Add(asignaturaEstudioIntegracion);
            gestorContext.SaveChanges();

            var resAsignaturaPlanIntegracion = CREATE.CrearAsignaturaPlanIntegracionSaveChange(gestorContext, erpContext);
            var otraAsignaturaPlanIntegracion = resAsignaturaPlanIntegracion.Item1;

            // Act
            var result = target.Modificar(new SaveAsignaturaIntegracionParameters
            {
                IdAsignatura = asignaturaEstudioIntegracion.Id,
                IdAsignaturaPlan = otraAsignaturaPlanIntegracion.Id
            });

            var persisted = gestorContext.AsignaturaIntegracion.Find(asignaturaEstudioIntegracion.Id); //First(ae => ae.Id == asignaturaEstudioIntegracion.Id);
            var hasUpdated = persisted.AsignaturaPlanIntegracionId == otraAsignaturaPlanIntegracion.Id;

            // Assert
            Assert.IsTrue(result.HasErrors);
            Assert.IsFalse(hasUpdated);
        }

        [TestMethod]
        public void EliminarTestCasoBueno1()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            gestorContext.AsignaturaIntegracion.Add(asignaturaEstudioIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] { asignaturaEstudioIntegracion.Id });

            // Assert
            Assert.IsTrue(!result.HasErrors);
        }

        [TestMethod]
        public void EliminarTestCasoBueno2()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            var otraAsignaturaEstudioIntegracion = PrepararAsignaturaEstudioIntegracion();
            gestorContext.AsignaturaIntegracion.Add(asignaturaEstudioIntegracion);
            gestorContext.AsignaturaIntegracion.Add(otraAsignaturaEstudioIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] { asignaturaEstudioIntegracion.Id, otraAsignaturaEstudioIntegracion.Id });

            // Assert
            Assert.IsTrue(!result.HasErrors);
        }

        [TestMethod]
        public void EliminarTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new AsignaturaIntegracionServices(gestorContext, erpContext);

            // Act
            var result = target.Eliminar(null);

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
    }
}
