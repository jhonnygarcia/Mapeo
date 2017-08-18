using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.PlantillaAsignaturaIntegracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class AsignaturaPlanIntegracionServicesTest : GestorMapeosServicesTest
    {
        private PlantillaAsignaturaIntegracion PrepararAsignaturaPlanIntegracion()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();

            var resPlanIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            var planIntegracion = resPlanIntegracion.Item1;
            var plan = resPlanIntegracion.Item2;

            var asignaturaPlan = CREATE.PrepararAsignaturaPlan();
            var plantillaAsignatura = CREATE.PrepararPlantillaAsignatura();
            plantillaAsignatura.PlantillaEstudio = planIntegracion.PlantillaEstudio;
            asignaturaPlan.Plan = plan;

            gestorContext.PlantillaAsignatura.Add(plantillaAsignatura);
            erpContext.AsignaturaPlan.Add(asignaturaPlan);
            erpContext.SaveChanges();
            gestorContext.SaveChanges();

            var persisted = new PlantillaAsignaturaIntegracion
            {
                Id = asignaturaPlan.Id,
                PlanIntegracionId = planIntegracion.Id,
                PlantillaAsignaturaId = plantillaAsignatura.Id
            };
            return persisted;
        }

        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);
            var asignaturaPlanIntegracion = PrepararAsignaturaPlanIntegracion();

            // Act
            var result = target.Crear(new SavePlantillaAsignaturaIntegracionParameters
            {
                IdAsignaturaPlan = asignaturaPlanIntegracion.Id,
                IdPlantillaAsignatura = asignaturaPlanIntegracion.PlantillaAsignaturaId
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
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            // Act
            var result = target.Crear(new SavePlantillaAsignaturaIntegracionParameters
            {
                IdAsignaturaPlan = random.Next(1, int.MaxValue),
                IdPlantillaAsignatura = random.Next(1, int.MaxValue)
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
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            var resPlanIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            var planIntegracion = resPlanIntegracion.Item1;

            var asignaturaPlan = CREATE.PrepararAsignaturaPlan();
            var plantillaAsignatura = CREATE.PrepararPlantillaAsignatura();

            plantillaAsignatura.PlantillaEstudio = planIntegracion.PlantillaEstudio;

            gestorContext.PlantillaAsignatura.Add(plantillaAsignatura);
            erpContext.AsignaturaPlan.Add(asignaturaPlan);
            erpContext.SaveChanges();
            gestorContext.SaveChanges();

            // Act
            var result = target.Crear(new SavePlantillaAsignaturaIntegracionParameters
            {
                IdAsignaturaPlan = asignaturaPlan.Id,
                IdPlantillaAsignatura = plantillaAsignatura.Id
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
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaPlanIntegracion = PrepararAsignaturaPlanIntegracion();
            gestorContext.PlantillaAsignaturaIntegracion.Add(asignaturaPlanIntegracion);
            gestorContext.SaveChanges();


            var otraPlantillaAsignatura = CREATE.PrepararPlantillaAsignatura();

            var plantillaEstudio = gestorContext.PlantillaEstudioIntegracion.Where(
                pi => pi.Id == asignaturaPlanIntegracion.PlanIntegracionId)
                .Select(a => a.PlantillaEstudio).First();

            otraPlantillaAsignatura.PlantillaEstudio = plantillaEstudio;
            gestorContext.PlantillaAsignatura.Add(otraPlantillaAsignatura);

            gestorContext.SaveChanges();
            
            // Act
            var result = target.Modificar(new SavePlantillaAsignaturaIntegracionParameters
            {
                IdAsignaturaPlan = asignaturaPlanIntegracion.Id,
                IdPlantillaAsignatura = otraPlantillaAsignatura.Id
            });

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void ModificarTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaPlanIntegracion = PrepararAsignaturaPlanIntegracion();
            gestorContext.PlantillaAsignaturaIntegracion.Add(asignaturaPlanIntegracion);
            gestorContext.SaveChanges();

            var otraPlantillaAsignatura = CREATE.PrepararPlantillaAsignatura();
            var plantillaEstudio = gestorContext.PlantillaEstudioIntegracion.Where(
                pi => pi.Id == asignaturaPlanIntegracion.PlanIntegracionId)
                .Select(a => a.PlantillaEstudio).First();

            otraPlantillaAsignatura.PlantillaEstudio = plantillaEstudio;
            gestorContext.PlantillaAsignatura.Add(otraPlantillaAsignatura);
            gestorContext.SaveChanges();
            // Act
            var result = target.Modificar(new SavePlantillaAsignaturaIntegracionParameters
            {
                IdAsignaturaPlan = asignaturaPlanIntegracion.Id,
                IdPlantillaAsignatura = otraPlantillaAsignatura.Id
            });

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }

        [TestMethod]
        public void EliminarTestCasoBueno1()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaPlanIntegracion = PrepararAsignaturaPlanIntegracion();
            gestorContext.PlantillaAsignaturaIntegracion.Add(asignaturaPlanIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] { asignaturaPlanIntegracion.Id });

            // Assert
            Assert.IsTrue(!result.HasErrors);
        }
        [TestMethod]
        public void EliminarTestCasoBueno2()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            var asignaturaPlanIntegracion = PrepararAsignaturaPlanIntegracion();
            var otraAsignaturaPlanIntegracion = PrepararAsignaturaPlanIntegracion();
            gestorContext.PlantillaAsignaturaIntegracion.Add(asignaturaPlanIntegracion);
            gestorContext.PlantillaAsignaturaIntegracion.Add(otraAsignaturaPlanIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] { asignaturaPlanIntegracion.Id, otraAsignaturaPlanIntegracion.Id });

            // Assert
            Assert.IsTrue(!result.HasErrors);
        }
        [TestMethod]
        public void EliminarTestCasoMalo1()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

            // Act
            var result = target.Eliminar(null);

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void EliminarTestCasoMalo2()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaAsignaturaIntegracionServices(gestorContext, erpContext);

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

            var asignaturaEstudio = new AsignaturaIntegracion
            {
                Id = asignaturaUnir.Id,
                AsignaturaPlanIntegracionId = asignaturaPlanIntegracion.Id
            };
            gestorContext.AsignaturaIntegracion.Add(asignaturaEstudio);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] {asignaturaPlanIntegracion.Id});

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
    }
}
