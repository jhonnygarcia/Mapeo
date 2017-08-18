using System;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class PeriodoEstudioIntegracionServicesTest : GestorMapeosServicesTest
    {
        private PeriodoEstudioIntegracion PrepararPeriodoEstudioIntegracion()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();

            var resPeriodoMatriculacion = CREATE.CrearPeriodoMatriculacionIntegracionSaveChanges(gestorContext, erpContext);
            var resEstudio = CREATE.CrearEstudioIntegracionSaveChanges(gestorContext, erpContext);
            //Gestor
            var periodoEstudio = CREATE.PrepararPeriodoEstudioUnir();
            periodoEstudio.EstudioUnir = resEstudio.Item2;
            periodoEstudio.PeriodoMatriculacionUnir = resPeriodoMatriculacion.Item2;
            gestorContext.PeriodoEstudioUnir.Add(periodoEstudio);
            gestorContext.SaveChanges();
            //Erp
            var planOfertado = CREATE.PrepararPlanOfertado();
            planOfertado.PeriodoAcademico = resPeriodoMatriculacion.Item3;
            planOfertado.Plan = resEstudio.Item5;
            erpContext.PlanOfertado.Add(planOfertado);
            erpContext.SaveChanges();

            var persisted = new PeriodoEstudioIntegracion
            {
                Id = periodoEstudio.Id,
                PlanOfertadoId = planOfertado.Id,
                PlantillaEstudioIntegracionId = planOfertado.PlanId
            };

            return persisted;
        }

        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);

            var periodoEstudioIntegracion = PrepararPeriodoEstudioIntegracion();
            // Act
            var result = target.Crear(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = periodoEstudioIntegracion.Id,
                IdPlanOfertado = periodoEstudioIntegracion.PlanOfertadoId
            });

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void CrearTestCasoMalo_ReferenciasNoExistentes()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            // Act
            var result = target.Crear(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = random.Next(1, int.MaxValue),
                IdPlanOfertado = random.Next(1, int.MaxValue)
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void CrearTestCasoMalo_PlanYEstudioNoMapeadosCorrectamente()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);
            var periodoEstudioIntegracion = PrepararPeriodoEstudioIntegracion();

            var otroPlanOfertado = CREATE.PrepararPlanOfertado();
            erpContext.PlanOfertado.Add(otroPlanOfertado);
            erpContext.SaveChanges();

            var otroPeriodoEstudio = CREATE.PrepararPeriodoEstudioUnir();
            gestorContext.PeriodoEstudioUnir.Add(otroPeriodoEstudio);
            gestorContext.SaveChanges();

            periodoEstudioIntegracion.PlanOfertadoId = otroPlanOfertado.Id;
            periodoEstudioIntegracion.Id = otroPeriodoEstudio.Id;

            // Act
            var result = target.Crear(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = periodoEstudioIntegracion.Id,
                IdPlanOfertado = periodoEstudioIntegracion.PlanOfertadoId
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void CrearTestCasoMalo_PeriodoMatriculacionNoMapeadoCorrectamente()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);
            var periodoEstudioIntegracion = PrepararPeriodoEstudioIntegracion();

            var periodoEstudioUnir = gestorContext.PeriodoEstudioUnir.Find(periodoEstudioIntegracion.Id);
            var periodoMatriculacionIntegracion = gestorContext.PeriodosMatriculacionesIntegracion.Find(periodoEstudioUnir.PeriodoMatriculacionId);
            gestorContext.PeriodosMatriculacionesIntegracion.Remove(periodoMatriculacionIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Crear(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = periodoEstudioIntegracion.Id,
                IdPlanOfertado = periodoEstudioIntegracion.PlanOfertadoId
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void ModificarTestCasoBueno()
        {
            //Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);

            //Get Data
            var periodoEstudioIntegracion = PrepararPeriodoEstudioIntegracion();
            gestorContext.PeriodoEstudioIntegracion.Add(periodoEstudioIntegracion);
            gestorContext.SaveChangeDetach();

            //Esta prueba es directa ya que logicamente no se puede enlazar con otro PlanOfertado sin evitar las validaciones
            //Act
            var result = target.Modificar(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = periodoEstudioIntegracion.Id,
                IdPlanOfertado = periodoEstudioIntegracion.PlantillaEstudioIntegracionId
            });

            //Assert 
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void ModificarTestCasoMalo_ReferenciasNoExistentes()
        {
            //Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            //Act
            var result = target.Modificar(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = random.Next(1, int.MaxValue),
                IdPlanOfertado = random.Next(1, int.MaxValue)
            });

            //Assert 
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void ModificarTestCasoMalo_PlanOfertadoNoTieneRelacionesMapeadasCorrectamente()
        {
            //Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);

            //Get Data
            var periodoEstudioIntegracion = PrepararPeriodoEstudioIntegracion();
            gestorContext.PeriodoEstudioIntegracion.Add(periodoEstudioIntegracion);
            gestorContext.SaveChangeDetach();

            var otroPlanOfertado = CREATE.PrepararPlanOfertado();
            erpContext.PlanOfertado.Add(otroPlanOfertado);
            erpContext.SaveChanges();
            
            //Act
            var result = target.Modificar(new SavePeriodoEstudioIntegracionParameters
            {
                IdPeriodoEstudio = periodoEstudioIntegracion.Id,
                IdPlanOfertado = otroPlanOfertado.Id
            });

            //Assert 
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void EliminarTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext,erpContext);
           
            var periodoEstudioIntegracion = CREATE.CrearPeriodoEstudioIntegracionSaveChanges(gestorContext, erpContext);


            //Act
            var result = target.Eliminar(new[] {periodoEstudioIntegracion.Item1.Id});

            //Assert
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void EliminarTestCasoMalo_CodigosNulos()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioIntegracionServices(gestorContext, erpContext);

            //Act
            var result = target.Eliminar(null);

            //Assert
            Assert.IsTrue(result.HasErrors);
        }
    }
}