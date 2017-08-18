using System;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.PeriodoEstudioAsignaturaIntegracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class PeriodoEstudioAsignaturaIntegracionServicesTest : GestorMapeosServicesTest
    {
        private PeriodoEstudioAsignaturaIntegracion PrepararPeriodoEstudioAsignaturaIntegracion()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();

            var resAsignaturaEstudioIntegracion = CREATE.CrearAsignaturaEstudioIntegracionSaveChanges(gestorContext, erpContext);
            var resEstudio = CREATE.CrearEstudioIntegracionSaveChanges(gestorContext, erpContext);
            var resPeriodoMatriculacion = CREATE.CrearPeriodoMatriculacionIntegracionSaveChanges(gestorContext, erpContext);
            var resPeriodoEstudio = CREATE.CrearPeriodoEstudioIntegracionSaveChanges(gestorContext, erpContext);

            //Ajustes previos en Gestor y ERP
            var asignaturaPlanIntegracion = resAsignaturaEstudioIntegracion.Item2;
            asignaturaPlanIntegracion.PlanIntegracion = resEstudio.Item4;

            var asignaturaUnir = resAsignaturaEstudioIntegracion.Item3;
            asignaturaUnir.EstudioUnir = resEstudio.Item2;

            var plantillaAsignatura = resAsignaturaEstudioIntegracion.Item4;
            plantillaAsignatura.PlantillaEstudio = resEstudio.Item3;

            var periodoEstudioIntegracion = resPeriodoEstudio.Item1;
            periodoEstudioIntegracion.PlantillaEstudioIntegracion = resEstudio.Item4;

            var periodoEstudioUnir = resPeriodoEstudio.Item2;
            periodoEstudioUnir.PeriodoMatriculacionUnir = resPeriodoMatriculacion.Item2;
            periodoEstudioUnir.EstudioUnir = resEstudio.Item2;

            var planOfertado = resPeriodoEstudio.Item3;
            planOfertado.PeriodoAcademico = resPeriodoMatriculacion.Item3;
            planOfertado.Plan = resEstudio.Item5;

            gestorContext.SaveChanges();
            erpContext.SaveChanges();

            //Gestor
            var periodoEstudioAsignaturaUnir = CREATE.PrepararPeriodoEstudioAsignaturaUnir();
            periodoEstudioAsignaturaUnir.AsignaturaUnir = asignaturaUnir;
            periodoEstudioAsignaturaUnir.PeriodoEstudioUnir = periodoEstudioUnir;
            gestorContext.PeriodoEstudioAsignaturaUnir.Add(periodoEstudioAsignaturaUnir);
            gestorContext.SaveChanges();

            //Erp
            var asignaturaOfertada = CREATE.PrepararAsignaturaOfertada();
            asignaturaOfertada.PlanOfertado = planOfertado;
            asignaturaOfertada.AsignaturaPlan = resAsignaturaEstudioIntegracion.Item5;
            asignaturaOfertada.AsignaturaPlan.Plan = resEstudio.Item5;
            erpContext.AsignaturaOfertada.Add(asignaturaOfertada);
            erpContext.SaveChanges();

            var persisted = new PeriodoEstudioAsignaturaIntegracion
            {
                Id = periodoEstudioAsignaturaUnir.Id,
                AsignaturaOfertadaId = asignaturaOfertada.Id
            };

            return persisted;
        }

        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            var periodoEstudioAsignaturaIntegracion = PrepararPeriodoEstudioAsignaturaIntegracion();
            // Act
            var result = target.Crear(new SavePeriodoEstudioAsignaturaIntegracionParameters
            {
                IdPeriodoEstudioAsignatura = periodoEstudioAsignaturaIntegracion.Id,
                IdAsignaturaOfertada = periodoEstudioAsignaturaIntegracion.AsignaturaOfertadaId
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
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            // Act
            var result = target.Crear(new SavePeriodoEstudioAsignaturaIntegracionParameters
            {
                IdPeriodoEstudioAsignatura = random.Next(1, int.MaxValue),
                IdAsignaturaOfertada = random.Next(1, int.MaxValue)
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void CrearTestCasoMalo_AsignaturaYPeriodoEstudioNoMapeadosCorrectamente()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            var periodoEstudioAsignaturaUnir = CREATE.PrepararPeriodoEstudioAsignaturaUnir();
            gestorContext.PeriodoEstudioAsignaturaUnir.Add(periodoEstudioAsignaturaUnir);
            gestorContext.SaveChanges();

            var asignaturaOfertada = CREATE.PrepararAsignaturaOfertada();
            erpContext.AsignaturaOfertada.Add(asignaturaOfertada);
            erpContext.SaveChanges();

            // Act
            var result = target.Crear(new SavePeriodoEstudioAsignaturaIntegracionParameters
            {
                IdPeriodoEstudioAsignatura = periodoEstudioAsignaturaUnir.Id,
                IdAsignaturaOfertada = asignaturaOfertada.Id
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
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            //Get Data
            var periodoEstudioAsignaturaIntegracion = PrepararPeriodoEstudioAsignaturaIntegracion();
            gestorContext.PeriodoEstudioAsignaturaIntegracion.Add(periodoEstudioAsignaturaIntegracion);
            gestorContext.SaveChanges();

            //Esta prueba es directa ya que logicamente no se puede enlazar con otro PlanOfertado sin evitar las validaciones
            //Act
            var result = target.Modificar(new SavePeriodoEstudioAsignaturaIntegracionParameters
            {
                IdPeriodoEstudioAsignatura = periodoEstudioAsignaturaIntegracion.Id,
                IdAsignaturaOfertada = periodoEstudioAsignaturaIntegracion.AsignaturaOfertadaId
            });

            //Assert 
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void ModificarTestCasoMalo_ReferenciasNoExistentes()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            var random = new Random();
            //Act
            var result = target.Modificar(new SavePeriodoEstudioAsignaturaIntegracionParameters
            {
                IdPeriodoEstudioAsignatura = random.Next(1, int.MaxValue),
                IdAsignaturaOfertada = random.Next(1, int.MaxValue)
            });

            //Assert 
            Assert.IsTrue(result.HasErrors);
        }
        [TestMethod]
        public void ModificarTestCasoMalo_ASignaturaOfertadaNoTieneRelacionesMapeadasCorrectamente()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            //Get Data
            var periodoEstudioAsignaturaIntegracion = PrepararPeriodoEstudioAsignaturaIntegracion();
            gestorContext.PeriodoEstudioAsignaturaIntegracion.Add(periodoEstudioAsignaturaIntegracion);
            gestorContext.SaveChanges();

            var otraAsignaturaOfertada = CREATE.PrepararAsignaturaOfertada();
            erpContext.AsignaturaOfertada.Add(otraAsignaturaOfertada);
            erpContext.SaveChanges();

            //Act
            var result = target.Modificar(new SavePeriodoEstudioAsignaturaIntegracionParameters
            {
                IdPeriodoEstudioAsignatura = periodoEstudioAsignaturaIntegracion.Id,
                IdAsignaturaOfertada = otraAsignaturaOfertada.Id
            });

            //Assert 
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void EliminarCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);

            var periodoEstudioAsignaturaIntegracion = PrepararPeriodoEstudioAsignaturaIntegracion();
            gestorContext.PeriodoEstudioAsignaturaIntegracion.Add(periodoEstudioAsignaturaIntegracion);
            gestorContext.SaveChanges();

            //Act 
            var result = target.Eliminar(new[] {periodoEstudioAsignaturaIntegracion.Id});
            //Assert

            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void EliminarCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoEstudioAsignaturaIntegracionServices(gestorContext, erpContext);
            var asignaturaOfertada = CREATE.PrepararAsignaturaOfertada();
            var periodoEstudioAsignatura = CREATE.PrepararPeriodoEstudioAsignaturaUnir();
            gestorContext.PeriodoEstudioAsignaturaUnir.Add(periodoEstudioAsignatura);
            erpContext.AsignaturaOfertada.Add(asignaturaOfertada);
            gestorContext.SaveChanges();
            erpContext.SaveChanges();

            var periodoEstudioAsignaturaIntegracion = new PeriodoEstudioAsignaturaIntegracion
            {
                Id = periodoEstudioAsignatura.Id,
                AsignaturaOfertadaId = asignaturaOfertada.Id
            };

            gestorContext.PeriodoEstudioAsignaturaIntegracion.Add(periodoEstudioAsignaturaIntegracion);
            gestorContext.SaveChanges();

            //Act 
            var result = target.Eliminar(new[] { 0, -1 });
            //Assert

            Assert.IsTrue(result.HasErrors);
        }
    }
}
