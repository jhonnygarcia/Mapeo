using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GestorMapeos.Models.Model.Erp20.Entities;

namespace GestorMapeos.Models.Model.Erp20
{
    public class PlanMapping : EntityTypeConfiguration<Plan>
    {
        public PlanMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdPlan");
            Property(a => a.EstudioId).HasColumnName("IdEstudio");
            Property(a => a.TituloId).HasColumnName("IdTitulo");

            //Relaciones a UNo
            HasRequired(a => a.Estudio).WithMany().HasForeignKey(a => a.EstudioId);
            HasRequired(a => a.Titulo).WithMany().HasForeignKey(a => a.TituloId);

            //Relaciones a Mucho
            HasMany(r => r.Nodos).WithRequired(i => i.Plan).HasForeignKey(i => i.PlanId);

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_plan.Planes");
        }
    }

    public class EstudioMapping : EntityTypeConfiguration<Estudio>
    {
        public EstudioMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdEstudio");
            Property(a => a.TipoEstudioId).HasColumnName("IdTipoEstudio");
            Property(a => a.RamaConocimientoId).HasColumnName("IdRamaConocimiento");

            HasRequired(a => a.TipoEstudio)
                .WithMany()
                .HasForeignKey(a => a.TipoEstudioId);

            HasRequired(a => a.RamaConocimiento)
                .WithMany()
                .HasForeignKey(a => a.RamaConocimientoId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_plan.Estudios");
        }
    }

    public class TipoEstudioMapping : EntityTypeConfiguration<TipoEstudio>
    {
        public TipoEstudioMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdTipoEstudio");

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("aca_plan.TiposEstudio");
        }
    }

    public class RamaConocimientoMapping : EntityTypeConfiguration<RamaConocimiento>
    {
        public RamaConocimientoMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdRamaConocimiento");

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("aca_plan.RamasConocimientos");
        }
    }

    public class TituloMapping : EntityTypeConfiguration<Titulo>
    {
        public TituloMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdTitulo");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_titulaciones.Titulos");
        }
    }

    public class NodoMapping : EntityTypeConfiguration<Nodo>
    {
        public NodoMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdNodo");
            Property(a => a.PlanId).HasColumnName("IdPlan");

            //Relaciones a Uno
            HasRequired(r => r.Plan).WithMany(i => i.Nodos).HasForeignKey(r => r.PlanId); 

            //Relaciones Muchos a Muchos
            this.HasMany(r1 => r1.Hitos).WithMany(r2 => r2.Nodos).Map(
                m =>
                {
                    m.MapLeftKey("IdNodo");
                    m.MapRightKey("IdHito");
                    m.ToTable("Nodos_Hitos", "aca_plan");
                });
            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("aca_plan.Nodos");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class HitoMapping : EntityTypeConfiguration<Hito>
    {
        public HitoMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdHito");

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("aca_plan.Hitos");
        }
    }

    public class HitoEspecializacionMapping : EntityTypeConfiguration<HitoEspecializacion>
    {
        public HitoEspecializacionMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdHito");
            Property(r => r.EspecializacionId).HasColumnName("IdEspecializacion");

            //Relaciones a Uno
            HasRequired(r => r.Especializacion).WithMany(i => i.Hitos).HasForeignKey(r => r.EspecializacionId);

            //Herencia simple
            HasRequired(r => r.Hito).WithOptional(t => t.HitoEspecializacion);

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("aca_plan.HitosEspecializaciones");
        }
    }

    public class EspecializacionMapping : EntityTypeConfiguration<Especializacion>
    {
        public EspecializacionMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdEspecializacion");
            Property(a => a.TituloId).HasColumnName("IdTitulo");

            //Relaciones a Uno
            HasRequired(a => a.Titulo).WithMany(a => a.Especializaciones).HasForeignKey(a => a.TituloId);

            //Relaciones Uno a Muchos
            HasMany(r => r.Hitos).WithRequired(i => i.Especializacion).HasForeignKey(i => i.EspecializacionId);
            
            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("aca_titulaciones.Especializaciones");
        }
    }

    public class AsignaturaMapping : EntityTypeConfiguration<Asignatura>
    {
        public AsignaturaMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdAsignatura");
            Property(a => a.TipoAsignaturaId).HasColumnName("IdTipoAsignatura");

            HasRequired(a => a.TipoAsignatura)
                .WithMany()
                .HasForeignKey(a => a.TipoAsignaturaId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_plan.Asignaturas");
        }
    }

    public class TipoAsignaturaMapping : EntityTypeConfiguration<TipoAsignatura>
    {
        public TipoAsignaturaMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdTipoAsignatura");

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("aca_plan.TiposAsignatura");
        }
    }

    public class DuracionPeriodoLectivoMapping : EntityTypeConfiguration<DuracionPeriodoLectivo>
    {
        public DuracionPeriodoLectivoMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdDuracionPeriodoLectivo");
            ToTable("aca_plan.DuracionesPeriodosLectivos");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

    public class CursoMapping : EntityTypeConfiguration<Curso>
    {
        public CursoMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdCurso");
            ToTable("aca_plan.Cursos");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class AsignaturaPlanMapping : EntityTypeConfiguration<AsignaturaPlan>
    {
        public AsignaturaPlanMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdAsignaturaPlan");
            Property(a => a.UbicacionPeriodoLectivo).HasColumnName("UbicacionPeriodoLectivo");
            Property(a => a.PlanId).HasColumnName("IdPlan");
            Property(a => a.AsignaturaId).HasColumnName("IdAsignatura");
            Property(a => a.DuracionPeriodoLectivoId).HasColumnName("IdDuracionPeriodoLectivo");
            Property(a => a.CursoId).HasColumnName("IdCurso");

            HasRequired(a => a.Plan)
                .WithMany()
                .HasForeignKey(a => a.PlanId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Asignatura)
                .WithMany()
                .HasForeignKey(a => a.AsignaturaId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.DuracionPeriodoLectivo)
                .WithMany()
                .HasForeignKey(a => a.DuracionPeriodoLectivoId);

            HasOptional(a => a.Curso)
                .WithMany()
                .HasForeignKey(a => a.CursoId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_plan.Asignaturas_Planes");
        }
    }

    #region Oferta Academica

    public class PeriodoAcademicoMapping : EntityTypeConfiguration<PeriodoAcademico>
    {
        public PeriodoAcademicoMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdPeriodoAcademico");
            Property(r => r.AnyoAcademicoId).HasColumnName("IdAnyoAcademico");

            //Relaciones a Uno
            HasRequired(r => r.AnyoAcademico).WithMany(i => i.PeriodosAcademicos).HasForeignKey(r => r.AnyoAcademicoId);

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("aca_oferta.PeriodosAcademicos");

            //Configuración de Llave Primaria Autonumérica.
            Property(ll => ll.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class AnyoAcademicoMapping : EntityTypeConfiguration<AnyoAcademico>
    {
        public AnyoAcademicoMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(r => r.Id).HasColumnName("IdAnyoAcademico");

            //Relaciones Uno a Muchos
            HasMany(r => r.PeriodosAcademicos).WithRequired(i => i.AnyoAcademico).HasForeignKey(i => i.AnyoAcademicoId);

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("aca_oferta.AnyosAcademicos");

            //Configuración de Llave Primaria Autonumérica.
            Property(ll => ll.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class PlanOfertadoMapping : EntityTypeConfiguration<PlanOfertado>
    {
        public PlanOfertadoMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdPlanOfertado");
            Property(a => a.PeriodoAcademicoId).HasColumnName("IdPeriodoAcademico");
            Property(a => a.PlanId).HasColumnName("IdPlan");

            HasRequired(a => a.PeriodoAcademico)
                .WithMany(a => a.PlanesOfertados)
                .HasForeignKey(a => a.PeriodoAcademicoId);

            HasRequired(a => a.Plan)
                .WithMany(a => a.PlanesOfertados)
                .HasForeignKey(a => a.PlanId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_oferta.PlanesOfertados");
        }
    }

    public class AsignaturaOfertadaMapping : EntityTypeConfiguration<AsignaturaOfertada>
    {
        public AsignaturaOfertadaMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdAsignaturaOfertada");
            Property(a => a.PlanOfertadoId).HasColumnName("IdPlanOfertado");
            Property(a => a.PeriodoLectivoId).HasColumnName("IdPeriodoLectivo");
            Property(a => a.AsignaturaPlanId).HasColumnName("IdAsignaturaPlan");
            Property(a => a.TipoAsignaturaId).HasColumnName("IdTipoAsignatura");
            Property(a => a.CursoId).HasColumnName("IdCurso");

            HasRequired(a => a.PeriodoLectivo)
                .WithMany(a => a.AsignaturasOfertadas)
                .HasForeignKey(a => a.PeriodoLectivoId);

            HasRequired(a => a.AsignaturaPlan)
                .WithMany(a => a.AsignaturasOfertadas)
                .HasForeignKey(a => a.AsignaturaPlanId);

            HasRequired(a => a.TipoAsignatura)
                .WithMany()
                .HasForeignKey(a => a.TipoAsignaturaId);

            HasOptional(a => a.Curso)
                .WithMany()
                .HasForeignKey(a => a.CursoId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_oferta.AsignaturasOfertadas");
        }
    }

    public class PeriodoLectivoMapping : EntityTypeConfiguration<PeriodoLectivo>
    {
        public PeriodoLectivoMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdPeriodoLectivo");
            Property(a => a.DuracionPeriodoLectivoId).HasColumnName("IdDuracionPeriodoLectivo");

            HasRequired(a => a.DuracionPeriodoLectivo)
                .WithMany()
                .HasForeignKey(a => a.DuracionPeriodoLectivoId)
                .WillCascadeOnDelete(false);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("aca_oferta.PeriodosLectivo");
        }
    }

    #endregion Oferta Academica
}