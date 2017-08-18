using System;
using System.Collections.Generic;
using System.Linq;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Erp20.Entities;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor.Impl;
using GestorMapeos.Parameters.EstudioIntegracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestorMapeos.Test.Gestor
{
    [TestClass]
    public class EstudioIntegracionServicesTest : GestorMapeosServicesTest
    {
        private EstudioIntegracion PrepararEstudioIntegracion()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();

            var resPlanIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            //Gestor
            var planIntegracion = resPlanIntegracion.Item1;
            var estudioUnir = CREATE.PrepararEstudioUnir();
            estudioUnir.PlantillasEstudio = new List<PlantillaEstudio>
            {
                planIntegracion.PlantillaEstudio
            };
            gestorContext.EstudioUnir.Add(estudioUnir);
            gestorContext.SaveChanges();

            //ERP
            var plan = resPlanIntegracion.Item2;
            var especializacion = new Especializacion
            {
                Nombre = Guid.NewGuid().ToString(),
                Titulo = new Titulo
                {
                    Nombre = Guid.NewGuid().ToString(),
                    CodigoMec = Guid.NewGuid().ToString()
                }
            };
            especializacion.Hitos = new List<HitoEspecializacion>();
            especializacion.Hitos.Add(new HitoEspecializacion
            {
                Hito = new Hito
                {
                    Nodos = new List<Nodo> { new Nodo { Plan = plan } }
                }
            });
            erpContext.Especializacion.Add(especializacion);
            erpContext.SaveChanges();

            // Act
            var persisted = new EstudioIntegracion
            {
                Id = estudioUnir.Id,
                EspecializacionId = especializacion.Id,
                PlantillaEstudioIntegracionId = planIntegracion.Id
            };
            
            return persisted;
        }

        [TestMethod]
        public void CrearTestCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new EstudioIntegracionServices(gestorContext, erpContext);

            var estudioIntegracion = PrepararEstudioIntegracion();
            // Act
            var result = target.Crear(new SaveEstudioIntegracionParameters
            {
               IdEstudioGestor = estudioIntegracion.Id,
               IdRefEspecializacion = estudioIntegracion.EspecializacionId,
               IdRefPlanErp = estudioIntegracion.PlantillaEstudioIntegracionId
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
            var target = new EstudioIntegracionServices(gestorContext, erpContext);
            var resPlanIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            var resOtroPlanIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            //Gestor
            var planIntegracion = resOtroPlanIntegracion.Item1;
            var estudioUnir = CREATE.PrepararEstudioUnir();
            estudioUnir.PlantillasEstudio = new List<PlantillaEstudio>
            {
                planIntegracion.PlantillaEstudio
            };
            gestorContext.EstudioUnir.Add(estudioUnir);
            gestorContext.SaveChanges();

            //ERP
            var otroPlan = CREATE.PrepararPlan();
            erpContext.Plan.Add(otroPlan);

            var especializacion = new Especializacion
            {
                Nombre = Guid.NewGuid().ToString(),
                Titulo = new Titulo
                {
                    Nombre = Guid.NewGuid().ToString(),
                    CodigoMec = Guid.NewGuid().ToString()
                }
            };
            especializacion.Hitos = new List<HitoEspecializacion>();
            especializacion.Hitos.Add(new HitoEspecializacion
            {
                Hito = new Hito
                {
                    Nodos = new List<Nodo> { new Nodo { Plan = otroPlan } }
                }
            });
            erpContext.Especializacion.Add(especializacion);
            erpContext.SaveChanges();
            // Act
            var result = target.Crear(new SaveEstudioIntegracionParameters
            {
                IdEstudioGestor = estudioUnir.Id,
                IdRefEspecializacion = especializacion.Id,
                IdRefPlanErp = planIntegracion.Id
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
            var target = new EstudioIntegracionServices(gestorContext, erpContext);

            var estudioIntegracion = PrepararEstudioIntegracion();
            gestorContext.EstudioIntegracion.Add(estudioIntegracion);
            gestorContext.SaveChanges();

            //ERP
            var especializacion = new Especializacion
            {
                Nombre = Guid.NewGuid().ToString(),
                Titulo = new Titulo
                {
                    Nombre = Guid.NewGuid().ToString(),
                    CodigoMec = Guid.NewGuid().ToString()
                },
                Hitos = new List<HitoEspecializacion>
                {
                    new HitoEspecializacion
                    {
                        Hito = new Hito
                        {
                            Nodos = new List<Nodo> {new Nodo {PlanId = estudioIntegracion.PlantillaEstudioIntegracionId } }
                        }
                    }
                }
            };
            erpContext.Especializacion.Add(especializacion);
            erpContext.SaveChanges();

           //Act
           var result = target.Modificar(new SaveEstudioIntegracionParameters
           {
               IdEstudioGestor = estudioIntegracion.Id,
               IdRefEspecializacion = especializacion.Id,
               IdRefPlanErp = estudioIntegracion.PlantillaEstudioIntegracionId
           });

            var persisted = gestorContext.EstudioIntegracion.Find(estudioIntegracion.Id);
            var hasModification = persisted.EspecializacionId != estudioIntegracion.Id;

            //Assert
            Assert.IsTrue(hasModification);
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void ModificarTestCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new EstudioIntegracionServices(gestorContext, erpContext);

            var estudioIntegracion = PrepararEstudioIntegracion();
            gestorContext.EstudioIntegracion.Add(estudioIntegracion);
            gestorContext.SaveChanges();

            var resPlanIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);

            // Act
            var result = target.Modificar(new SaveEstudioIntegracionParameters
            {
                IdEstudioGestor = estudioIntegracion.Id,
                IdRefEspecializacion = estudioIntegracion.EspecializacionId,
                IdRefPlanErp = resPlanIntegracion.Item1.Id
            });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        public void ElmininarCasoBueno()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new EstudioIntegracionServices(gestorContext, erpContext);

            var estudioIntegracion = PrepararEstudioIntegracion();
            gestorContext.EstudioIntegracion.Add(estudioIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] {estudioIntegracion.Id});

            // Assert
            Assert.IsTrue(result.HasErrors == false);
        }
        [TestMethod]
        public void ElmininarCasoMalo()
        {
            // Prepare
            var gestorContext = new GestorContext();
            var erpContext = new ErpContext();
            var target = new EstudioIntegracionServices(gestorContext, erpContext);

            var estudioIntegracion = PrepararEstudioIntegracion();
            gestorContext.EstudioIntegracion.Add(estudioIntegracion);
            gestorContext.SaveChanges();

            // Act
            var result = target.Eliminar(new[] { -1 });

            // Assert
            Assert.IsTrue(result.HasErrors);
        }
    }
}
