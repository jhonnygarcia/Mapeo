using System;
using System.Collections.Generic;
using System.Linq;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class PlanIntegracionServicesTest : GestorMapeosServicesTest
    {
        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaEstudioIntegracionServices(gestorContext, erpContext);

            var plan = CREATE.PrepararPlan();
            var plantillaEstudio = CREATE.PrepararPlantillaEstudio();

            erpContext.Plan.Add(plan);
            gestorContext.PlantillaEstudio.Add(plantillaEstudio);
            gestorContext.SaveChanges();
            erpContext.SaveChanges();
            // Act
            var result = target.Crear(new SavePlantillaEstudioIntegracionParameters
            {
                IdPlan = plan.Id,
                IdPlantillaEstudio = plantillaEstudio.Id
            });

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void CrearTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaEstudioIntegracionServices(gestorContext, erpContext);

            var planIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            // Act
            var result = target.Crear(new SavePlantillaEstudioIntegracionParameters
            {
                IdPlan = planIntegracion.Item1.Id,
                IdPlantillaEstudio = planIntegracion.Item1.PlantillaEstudioId
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
            var target = new PlantillaEstudioIntegracionServices(gestorContext, erpContext);
            var planIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);

            var plantillaEstudio = CREATE.PrepararPlantillaEstudio();
            gestorContext.PlantillaEstudio.Add(plantillaEstudio);
            gestorContext.SaveChanges();
            // Act
            var result = target.Modificar(new SavePlantillaEstudioIntegracionParameters
            {
                IdPlan = planIntegracion.Item1.Id,
                IdPlantillaEstudio = plantillaEstudio.Id
            });

            var persisted = gestorContext.PlantillaEstudioIntegracion.First(a => a.Id == planIntegracion.Item1.Id);
            var hasUpdate = persisted.PlantillaEstudioId == plantillaEstudio.Id;

            // Assert
            Assert.IsTrue(hasUpdate);
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void ModificarTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaEstudioIntegracionServices(gestorContext, erpContext);


            var planIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);

            // Act
            var result = target.Modificar(new SavePlantillaEstudioIntegracionParameters
            {
                IdPlan = planIntegracion.Item1.Id,
                IdPlantillaEstudio = 0
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void EliminarTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaEstudioIntegracionServices(gestorContext, erpContext);

            var planIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            // Act
            var result = target.Eliminar(new []{planIntegracion.Item1.Id});

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void EliminarTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PlantillaEstudioIntegracionServices(gestorContext, erpContext);

            var plantillaAsignatura = CREATE.PrepararPlantillaAsignatura();
            var asignatura = CREATE.PrepararAsignatura();

            gestorContext.PlantillaAsignatura.Add(plantillaAsignatura);
            erpContext.Asignatura.Add(asignatura);
            gestorContext.SaveChanges();
            erpContext.SaveChanges();

            var planIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            planIntegracion.Item1.AsignaturasIntegracion = new List<PlantillaAsignaturaIntegracion>();
            planIntegracion.Item1.AsignaturasIntegracion.Add(new PlantillaAsignaturaIntegracion
            {
                Id = asignatura.Id,
                PlantillaAsignatura = plantillaAsignatura
            });

            gestorContext.SaveChanges();
            // Act
            var result = target.Eliminar(new[] { planIntegracion.Item1.Id });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
    }
}
