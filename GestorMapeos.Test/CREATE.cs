using System;
using System.Collections.Generic;
using System.Data.Entity;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Erp20.Entities;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;

namespace GestorMapeos.Test
{
    public class CREATE
    {
        //ERP_ACADEMICO
        public static Plan PrepararPlan()
        {
            var plan = new Plan
            {
                Anyo = 2016,
                EsOficial = true,
                Codigo = Guid.NewGuid().ToString(),
                Nombre = Guid.NewGuid().ToString(),
                Titulo = new Titulo
                {
                    CodigoMec = Guid.NewGuid().ToString(),
                    Nombre = Guid.NewGuid().ToString()
                },
                Estudio = new Estudio
                {
                    Id = IDENTITY.ID_ESTUDIO,
                    CodigoRuct = Guid.NewGuid().ToString(),
                    Nombre = Guid.NewGuid().ToString(),
                    RamaConocimiento = new RamaConocimiento
                    {
                        Id = IDENTITY.ID_RAMA_CONOCIMIENTO,
                        Nombre = Guid.NewGuid().ToString()
                    },
                    TipoEstudio = new TipoEstudio
                    {
                        Id = IDENTITY.ID_TIPO_ESTUDIO,
                        Nombre = Guid.NewGuid().ToString()
                    }
                }
            };
            return plan;
        }
        public static Asignatura PrepararAsignatura()
        {
            var asignatura = new Asignatura
            {
                Nombre = Guid.NewGuid().ToString(),
                Codigo = Guid.NewGuid().ToString(),
                Creditos = 1,
                TipoAsignatura = new TipoAsignatura
                {
                    Id = IDENTITY.ID_TIPO_ASIGNATURA,
                    Nombre = Guid.NewGuid().ToString(),
                }
            };
            return asignatura;
        }
        public static AsignaturaPlan PrepararAsignaturaPlan()
        {
            var plan = PrepararPlan();
            var asignatura = PrepararAsignatura();
            var persisted = new AsignaturaPlan
            {
                Plan = plan,
                Asignatura = asignatura,
                UbicacionPeriodoLectivo = Int32.MaxValue,
                DuracionPeriodoLectivo = new DuracionPeriodoLectivo
                {
                    Id = IDENTITY.ID_DURACION_PERIODO_LECTIVO,
                    Nombre = Guid.NewGuid().ToString()
                },
                Curso = new Curso
                {
                    Numero = new Random(Environment.TickCount).Next()
                }
            };
            return persisted;
        }
        
        //GESTOR
        public static PlantillaAsignatura PrepararPlantillaAsignatura()
        {
            var plantillaEstudio = PrepararPlantillaEstudio();
            var plantillaAsignatura = new PlantillaAsignatura
            {
                Codigo = Guid.NewGuid().ToString(),
                NombreAsignatura = Guid.NewGuid().ToString(),
                Creditos = 1,
                PlantillaEstudio = plantillaEstudio,
                TipoAsignatura = new TipoAsignaturaUnir
                {
                    Id = IDENTITY.ID_TIPO_ASIGNATURA_UNIR,
                    Nombre = Guid.NewGuid().ToString()
                }
            };
            return plantillaAsignatura;
        }
        public static PlantillaEstudio PrepararPlantillaEstudio()
        {
            var plantillaEstudio = new PlantillaEstudio
            {
                Nombre = Guid.NewGuid().ToString(),
                AnyoPlan = 2016,
                TipoEstudio = new TipoEstudioUnir
                {
                    Id = IDENTITY.ID_TIPO_ESTUDIO_UNIR,
                    Nombre = Guid.NewGuid().ToString()
                },
                Rama =  new RamaUnir
                {
                    Nombre = Guid.NewGuid().ToString()
                }
            };
            return plantillaEstudio;
        }
        public static Tuple<PlantillaEstudioIntegracion, Plan>  CrearPlanIntegracionSaveChange(GestorContext gestorContext, ErpContext erpContext)
        {
            var plan = PrepararPlan();
            erpContext.Plan.Add(plan);
            erpContext.SaveChanges();

            var plantillaEstudio = PrepararPlantillaEstudio();
            var persisted = new PlantillaEstudioIntegracion
            {
                Id = plan.Id,
                PlantillaEstudio = plantillaEstudio
            };
            gestorContext.PlantillaEstudioIntegracion.Add(persisted);
            gestorContext.SaveChanges();
            return new Tuple<PlantillaEstudioIntegracion, Plan>(persisted, plan);
        }
        public static EstudioUnir PrepararEstudioUnir()
        {
            var persisted = new EstudioUnir
            {
                Nombre = Guid.NewGuid().ToString(),
                Activo = "",
                RamaEstudio = Guid.NewGuid().ToString(),
                PlanEstudio = Guid.NewGuid().ToString(),
                TipoEstudioSegunUnir = new TipoEstudioSegunUnir
                {
                    Id = IDENTITY.ID_TIPO_ESTUDIO_SEGUN_UNIR,
                    Nombre = Guid.NewGuid().ToString()
                }
            };
            return persisted;
        }
        
        public static PeriodoMatriculacionUnir PrepararPeriodoMatriculacionUnir()
        {
            var persisted = new PeriodoMatriculacionUnir
            {
                Nombre = Guid.NewGuid().ToString(),
                AnyoAcademico = Guid.NewGuid().ToString()
            };
            return persisted;
        }
        
        public static Tuple<PeriodoMatriculacionIntegracion, PeriodoMatriculacionUnir, PeriodoAcademico>
            CrearPeriodoMatriculacionIntegracionSaveChanges(GestorContext gestorContext, ErpContext erpContext)
        {
            var periodoAcademico = PrepararPeriodoAcademico();
            erpContext.PeriodosAcademicos.Add(periodoAcademico);
            erpContext.SaveChanges();

            var periodoMatriculacion = PrepararPeriodoMatriculacionUnir();
            var persisted = new PeriodoMatriculacionIntegracion
            {
                PeriodoMatriculacion = periodoMatriculacion,
                PeriodoAcademicoId = periodoAcademico.Id
            };
            gestorContext.PeriodosMatriculacionesIntegracion.Add(persisted);
            gestorContext.SaveChanges();

            return new Tuple<PeriodoMatriculacionIntegracion, PeriodoMatriculacionUnir, PeriodoAcademico>(persisted,
                periodoMatriculacion, periodoAcademico);
        }

        public static Tuple<EstudioIntegracion, EstudioUnir, PlantillaEstudio, PlantillaEstudioIntegracion, Plan>
            CrearEstudioIntegracionSaveChanges(GestorContext gestorContext, ErpContext erpContext)
        {
            var plan = PrepararPlan();
            erpContext.Plan.Add(plan);
            erpContext.SaveChanges();

            var estudio = PrepararEstudioUnir();
            var plantillaEstudio = PrepararPlantillaEstudio();
            plantillaEstudio.EstudiosUnir = new List<EstudioUnir> { estudio };
            var planIntegracion = new PlantillaEstudioIntegracion
            {
                Id = plan.Id,
                PlantillaEstudio = plantillaEstudio
            };
            var persisted = new EstudioIntegracion
            {
                EstudioUnir = estudio,
                PlantillaEstudioIntegracion = planIntegracion
            };
            gestorContext.EstudioIntegracion.Add(persisted);
            gestorContext.SaveChanges();

            return new Tuple<EstudioIntegracion, EstudioUnir, PlantillaEstudio, PlantillaEstudioIntegracion, Plan>(persisted, estudio, plantillaEstudio, planIntegracion, plan);
        }

        public static Tuple<PlantillaAsignaturaIntegracion, PlantillaAsignatura, AsignaturaPlan> 
            CrearAsignaturaPlanIntegracionSaveChange(GestorContext gestorContext, ErpContext erpContext)
        {
            var resPlanIntegracion = CrearPlanIntegracionSaveChange(gestorContext, erpContext);
            var planIntegracion = resPlanIntegracion.Item1;
            var plan = resPlanIntegracion.Item2;

            var asignaturaPlan = PrepararAsignaturaPlan();
            var plantillaAsignatura = PrepararPlantillaAsignatura();
            plantillaAsignatura.PlantillaEstudio = planIntegracion.PlantillaEstudio;
            asignaturaPlan.Plan = plan;

            gestorContext.PlantillaAsignatura.Add(plantillaAsignatura);
            gestorContext.SaveChanges();
            erpContext.AsignaturaPlan.Add(asignaturaPlan);
            erpContext.SaveChanges();

            var persisted = new PlantillaAsignaturaIntegracion
            {
                Id = asignaturaPlan.Id,
                PlanIntegracionId = planIntegracion.Id,
                PlantillaAsignaturaId = plantillaAsignatura.Id
            };
            gestorContext.PlantillaAsignaturaIntegracion.Add(persisted);
            gestorContext.SaveChanges();

            return new Tuple<PlantillaAsignaturaIntegracion, PlantillaAsignatura, AsignaturaPlan>(persisted, plantillaAsignatura, asignaturaPlan);
        }

        public static Tuple<AsignaturaIntegracion, PlantillaAsignaturaIntegracion, AsignaturaUnir, PlantillaAsignatura, AsignaturaPlan>
            CrearAsignaturaEstudioIntegracionSaveChanges(GestorContext gestorContext, ErpContext erpContext)
        {
            var resAsignaturaPlan = CrearAsignaturaPlanIntegracionSaveChange(gestorContext, erpContext);
            var asignaturaPlanIntegracion = resAsignaturaPlan.Item1;
            var plantillaAsignatura = resAsignaturaPlan.Item2;
            var asignaturaUnir = PrepararAsignaturaUnir();
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
            gestorContext.AsignaturaIntegracion.Add(persisted);
            gestorContext.SaveChanges();

            return
                new Tuple<AsignaturaIntegracion, PlantillaAsignaturaIntegracion, AsignaturaUnir, PlantillaAsignatura, AsignaturaPlan>(
                    persisted, asignaturaPlanIntegracion, asignaturaUnir, plantillaAsignatura, resAsignaturaPlan.Item3);
        }
        
        #region Asignatura Estudio Integracion

        public static AsignaturaUnir PrepararAsignaturaUnir()
        {
            var estudioUnir = PrepararEstudioUnir();
            var persisted = new AsignaturaUnir
            {
                Nombre = Guid.NewGuid().ToString(),
                TipoAsignatura = Guid.NewGuid().ToString(),
                Creditos = 1,
                PeriodoLectivo = Guid.NewGuid().ToString(),
                Curso = 2,
                Activo = "",
                EstudioUnir = estudioUnir
            };
            return persisted;
        }

        #endregion

        #region Periodo Estudio Integracion

        public static PeriodoEstudioUnir PrepararPeriodoEstudioUnir()
        {
            var estudio = PrepararEstudioUnir();
            var periodoMatriculacion = PrepararPeriodoMatriculacionUnir();
            var persisted = new PeriodoEstudioUnir
            {
                EstudioUnir = estudio,
                PeriodoMatriculacionUnir = periodoMatriculacion,
                FechaInicio = DateTime.Now
            };
            return persisted;
        }

        public static PlanOfertado PrepararPlanOfertado()
        {
            var plan = PrepararPlan();
            var periodoAcademico = PrepararPeriodoAcademico();
            var persisted = new PlanOfertado
            {
                PeriodoAcademico = periodoAcademico,
                Plan = plan
            };
            return persisted;
        }

        public static Tuple<PeriodoEstudioIntegracion, PeriodoEstudioUnir, PlanOfertado> CrearPeriodoEstudioIntegracionSaveChanges(
            GestorContext gestorContext, ErpContext erpContext)
        {
            var periodoEstudioUnir = PrepararPeriodoEstudioUnir();
            var planOfertado = PrepararPlanOfertado();

            gestorContext.PeriodoEstudioUnir.Add(periodoEstudioUnir);
            erpContext.PlanOfertado.Add(planOfertado);

            gestorContext.SaveChanges();
            erpContext.SaveChanges();

            var planIntegracion = CREATE.CrearPlanIntegracionSaveChange(gestorContext, erpContext);

            var persisted = new PeriodoEstudioIntegracion
            {
                Id = periodoEstudioUnir.Id,
                PlanOfertadoId = planOfertado.Id,
                PlantillaEstudioIntegracionId = planIntegracion.Item1.Id 
            };
            gestorContext.PeriodoEstudioIntegracion.Add(persisted);
            gestorContext.SaveChanges();
            return new Tuple<PeriodoEstudioIntegracion, PeriodoEstudioUnir, PlanOfertado>(persisted, periodoEstudioUnir, planOfertado);

        }

        #endregion Periodo Estudio Integracion

        #region Periodo Matriculacion

        public static AnyoAcademico PrepararAnyoAcademico()
        {
            var persisted = new AnyoAcademico
            {
                AnyoInicio = 2016,
                AnyoFin = 2025,
            };
            return persisted;
        }

        public static PeriodoAcademico PrepararPeriodoAcademico()
        {
            var añoAcademico = PrepararAnyoAcademico();
            var persisted = new PeriodoAcademico
            {
                Nombre = Guid.NewGuid().ToString(),
                FechaInicio = DateTime.Now.Date,
                FechaFin = DateTime.Now.Date.AddMonths(18),
                AnyoAcademico = añoAcademico
            };
            return persisted;
        }
        
        public static Tuple<PeriodoMatriculacionIntegracion, PeriodoAcademico> CrearPeriodoMatriculacionSaveChanges(
            GestorContext gestorContext, ErpContext erpContext)
        {
            var periodoMatriculacionUnir = PrepararPeriodoMatriculacionUnir();
            var periodoAcademico = PrepararPeriodoAcademico();

            gestorContext.PeriodosMatriculacionesUnir.Add(periodoMatriculacionUnir);
            erpContext.PeriodosAcademicos.Add(periodoAcademico);

            gestorContext.SaveChanges();
            erpContext.SaveChanges();

            var persisted = new PeriodoMatriculacionIntegracion
            {
                Id = periodoMatriculacionUnir.Id,
                PeriodoAcademicoId = periodoAcademico.Id
            };
            gestorContext.PeriodosMatriculacionesIntegracion.Add(persisted);
            gestorContext.SaveChanges();
            return new Tuple<PeriodoMatriculacionIntegracion, PeriodoAcademico>(persisted,periodoAcademico);
        }

        #endregion

        #region Periodo Estudio Asignatura Integracion

        public static PeriodoEstudioAsignaturaUnir PrepararPeriodoEstudioAsignaturaUnir()
        {
            var periodoEstudioUnir = PrepararPeriodoEstudioUnir();
            var asignaturaUnir = PrepararAsignaturaUnir();
            var persisted = new PeriodoEstudioAsignaturaUnir
            {
                PeriodoEstudioUnir = periodoEstudioUnir,
                AsignaturaUnir = asignaturaUnir
            };
            return persisted;
        }

        public static AsignaturaOfertada PrepararAsignaturaOfertada()
        {
            var asignaturaPlan = PrepararAsignaturaPlan();
            var planOfertado = PrepararPlanOfertado();
            var persisted = new AsignaturaOfertada
            {
                Nombre = Guid.NewGuid().ToString(),
                Codigo = Guid.NewGuid().ToString(),
                UbicacionPeriodoLectivo = 1,
                Curso = new Curso
                {
                    Numero = 1
                },
                TipoAsignatura = new TipoAsignatura
                {
                    Id = IDENTITY.ID_TIPO_ASIGNATURA,
                    Nombre = Guid.NewGuid().ToString()
                },
                AsignaturaPlan = asignaturaPlan,
                PlanOfertado = planOfertado,
                PeriodoLectivo = new PeriodoLectivo
                {
                    Nombre = Guid.NewGuid().ToString(),
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now,
                    DuracionPeriodoLectivo = new DuracionPeriodoLectivo
                    {
                        Id = IDENTITY.ID_DURACION_PERIODO_LECTIVO,
                        Nombre = Guid.NewGuid().ToString()
                    }
                }
            };
            return persisted;
        }

        #endregion

        public void DetachContext(DbContext context)
        {
            //var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            //var objectStateEntries = objectContext
            //    .ObjectStateManager
            //    .GetObjectStateEntries(EntityState.Added);

            //foreach (var objectStateEntry in objectStateEntries)
            //{
            //    objectContext.Detach(objectStateEntry.Entity);
            //}            
        }
    }
}

