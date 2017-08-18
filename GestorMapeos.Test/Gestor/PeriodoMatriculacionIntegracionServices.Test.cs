using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.PeriodoMatriculacionIntegracion;
using GestorMapeos.TransferStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class PeriodoMatriculacionIntegracionServicesTest : GestorMapeosServicesTest
    {
        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoMatriculacionIntegracionServices(gestorContext, erpContext);

            var periodoMatriculacion = CREATE.PrepararPeriodoMatriculacionUnir();
            var periodoAcademico = CREATE.PrepararPeriodoAcademico();

            gestorContext.PeriodosMatriculacionesUnir.Add(periodoMatriculacion);
            erpContext.PeriodosAcademicos.Add(periodoAcademico);
            erpContext.SaveChanges();
            gestorContext.SaveChanges();


            //Act 
            var result = target.Crear(new SavePeriodoMatriculacionIntegracionParameters
            {
                IdPeriodoMatriculacionGestor = periodoMatriculacion.Id,
                IdRefPeriodoAcademicoErp = periodoAcademico.Id
            });

            //Assert
            Assert.IsTrue(result.HasErrors == false);
        }

        [TestMethod]
        public void CrearTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoMatriculacionIntegracionServices(gestorContext, erpContext);

            var periodoMatriculacionIntegracion = CREATE.CrearPeriodoMatriculacionSaveChanges(gestorContext, erpContext);

            //Act
            var result = target.Crear(new SavePeriodoMatriculacionIntegracionParameters
            {
                IdPeriodoMatriculacionGestor = periodoMatriculacionIntegracion.Item1.Id,
                IdRefPeriodoAcademicoErp = periodoMatriculacionIntegracion.Item1.PeriodoAcademicoId
            });

            //Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void ModificarTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoMatriculacionIntegracionServices(gestorContext, erpContext);

            var periodoMatriculacionIntegracion = CREATE.CrearPeriodoMatriculacionSaveChanges(gestorContext, erpContext);

            var periodoAcademico = CREATE.PrepararPeriodoAcademico();
            erpContext.PeriodosAcademicos.Add(periodoAcademico);
            erpContext.SaveChanges();

            //Act
            var result = target.Modificar(new SavePeriodoMatriculacionIntegracionParameters
            {
                IdPeriodoMatriculacionGestor = periodoMatriculacionIntegracion.Item1.Id,
                IdRefPeriodoAcademicoErp = periodoAcademico.Id
            });

            //Assert
            Assert.IsTrue(result.HasErrors == false);
        }

        [TestMethod]
        public void ModificarTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new PeriodoMatriculacionIntegracionServices(gestorContext, erpContext);

            var periodoMatriculacionIntegracion = CREATE.CrearPeriodoMatriculacionSaveChanges(gestorContext, erpContext);

            //Act
            var result = target.Modificar(new SavePeriodoMatriculacionIntegracionParameters
            {
                IdPeriodoMatriculacionGestor = periodoMatriculacionIntegracion.Item1.Id,
                IdRefPeriodoAcademicoErp = 0
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
            var target = new PeriodoMatriculacionIntegracionServices(gestorContext, erpContext);

            var periodoMatriculacionIntegracion = CREATE.CrearPeriodoMatriculacionSaveChanges(gestorContext, erpContext);

            //Act
            var result = target.Eliminar(new []{ periodoMatriculacionIntegracion.Item1.Id});

            //Assert
            Assert.IsTrue(result.HasErrors == false);

        }

        [TestMethod]
        public void EliminarTestCasoMalo()
        {
           // throw new NotImplementedException();
        }
    }
}