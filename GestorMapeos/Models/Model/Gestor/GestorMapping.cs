using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GestorMapeos.Models.Model.Gestor.Entities;

namespace GestorMapeos.Models.Model.Gestor
{
    public class PlantillaAsignaturaMapping : EntityTypeConfiguration<PlantillaAsignatura>
    {
        public PlantillaAsignaturaMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdPlantillaAsignatura");
            Property(a => a.Creditos).HasColumnName("ECTS");
            Property(a => a.PlantillaEstudioId).HasColumnName("IdPlantillaEstudio");
            Property(a => a.TipoAsignaturaId).HasColumnName("IdTipoAsignatura");

            HasRequired(a => a.PlantillaEstudio)
                .WithMany(a => a.PlantillasAsignatura)
                .HasForeignKey(a => a.PlantillaEstudioId)
                .WillCascadeOnDelete(false);

            HasMany(a => a.Asignaturas)
                .WithMany(a => a.PlantillasAsignaturas)
                .Map(a =>
                {
                    a.MapLeftKey("IdPlantillaAsignatura");
                    a.MapRightKey("IdAsignaturaUnir");
                    a.ToTable("TbAsignaturaUnir_PlantillaAsignatura", "Expedientes");
                });

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Expedientes.PlantillaAsignatura");
        }
    }
    public class AsignaturaPlanIntegracionMapping : EntityTypeConfiguration<PlantillaAsignaturaIntegracion>
    {
        public AsignaturaPlanIntegracionMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdRefAsignaturaPlanERP");
            Property(a => a.PlantillaAsignaturaId).HasColumnName("IdPlantillaAsignatura");
            Property(a => a.PlanIntegracionId).HasColumnName("IdRefPlanERP");

            HasRequired(a => a.PlantillaAsignatura)
                .WithMany()
                .HasForeignKey(a => a.PlantillaAsignaturaId);

            HasRequired(a => a.PlanIntegracion)
                .WithMany(a => a.AsignaturasIntegracion)
                .HasForeignKey(a => a.PlanIntegracionId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("IntegracionERPAca.AsignaturasPlanERP");
        }
    }
    public class AsignaturaUnirMapping : EntityTypeConfiguration<AsignaturaUnir>
    {
        public AsignaturaUnirMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("idUNIRGAAsignaturaEstudio");
            Property(a => a.Nombre).HasColumnName("sDenominacionAsignatura");
            Property(a => a.TipoAsignatura).HasColumnName("sCaracterAsignatura");
            Property(a => a.Creditos).HasColumnName("iNumeroECTS");
            Property(a => a.PeriodoLectivo).HasColumnName("sPeriodoLectivo");
            Property(a => a.Curso).HasColumnName("iNumeroAnyoCurso");
            Property(a => a.Activo).HasColumnName("cActivo");
            Property(a => a.Borrado).HasColumnName("cBorrado");
            Property(a => a.EstudioUnirId).HasColumnName("idUNIRGAEstudio");

            HasRequired(a => a.EstudioUnir)
                .WithMany(a => a.AsignaturasUnir)
                .HasForeignKey(a => a.EstudioUnirId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("dbo.tb_UNIRGA_AsignaturasEstudios");
        }
    }

    public class AsignaturaEstudioIntegracionMapping : EntityTypeConfiguration<AsignaturaIntegracion>
    {
        public AsignaturaEstudioIntegracionMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdAsignaturaEstudioGestor");
            Property(a => a.AsignaturaPlanIntegracionId).HasColumnName("IdRefAsignaturaPlanERP");

            HasRequired(r => r.AsignaturaUnir).WithOptional(t => t.AsignaturaEstudioIntegracion);

            HasRequired(a => a.AsignaturaPlanIntegracion)
                .WithMany(a => a.AsignaturasEstudioIntegracion)
                .HasForeignKey(a => a.AsignaturaPlanIntegracionId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("IntegracionERPAca.AsignaturasEstudioGestor");
        }
    }
    public class EstudioUnirMapping : EntityTypeConfiguration<EstudioUnir>
    {
        public EstudioUnirMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("idUNIRGAEstudios");
            Property(a => a.Nombre).HasColumnName("sEstudioUNIR");
            Property(a => a.PlanEstudio).HasColumnName("sPlanEstudio");
            Property(a => a.RamaEstudio).HasColumnName("sRamaEstudio");
            Property(a => a.Activo).HasColumnName("cActivo");
            Property(a => a.Borrado).HasColumnName("cBorrado");

            Property(a => a.TipoEstudioSegunUnirId).HasColumnName("iCodTipoEstudioSegunUNIR");
            Property(a => a.EstudioPrincipalUnirId).HasColumnName("idEstudioPrincipal");

            //Relaciones a Uno
            HasRequired(a => a.TipoEstudioSegunUnir).WithMany().HasForeignKey(a => a.TipoEstudioSegunUnirId);
            HasOptional(a => a.EstudioPrincipalUnir).WithMany().HasForeignKey(a => a.EstudioPrincipalUnirId);

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("dbo.tb_UNIRGA_Estudios");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class EstudioPrincipalUnirMapping : EntityTypeConfiguration<EstudioPrincipalUnir>
    {
        public EstudioPrincipalUnirMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("idEstudioPrincipal");
            Property(a => a.Codigo).HasColumnName("cTitulacion");
            Property(a => a.Nombre).HasColumnName("sNombreTitulacion");

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("dbo.tb_UNIRGA_EstudiosPrincipales");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class PlantillaEstudioMapping : EntityTypeConfiguration<PlantillaEstudio>
    {
        public PlantillaEstudioMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdPlantillaEstudio");
            Property(a => a.Nombre).HasColumnName("NombreEstudio");
            Property(a => a.RamaId).HasColumnName("IdRama");
            Property(a => a.TipoEstudioId).HasColumnName("IdTipoEstudio");

            HasRequired(a => a.TipoEstudio)
                .WithMany()
                .HasForeignKey(a => a.TipoEstudioId);

            HasOptional(a => a.Rama)
                .WithMany()
                .HasForeignKey(a => a.RamaId);

            HasMany(a => a.EstudiosUnir)
                .WithMany(a => a.PlantillasEstudio)
                .Map(a =>
                {
                    a.MapLeftKey("IdPlantillaEstudio");
                    a.MapRightKey("IdEstudioUnir");
                    a.ToTable("TbEstudioUnir_PlantillaEstudio", "Expedientes");
                });

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Expedientes.PlantillaEstudio");
        }
    }
    public class PlanIntegracionMapping : EntityTypeConfiguration<PlantillaEstudioIntegracion>
    {
        public PlanIntegracionMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdRefPlanERP");
            Property(a => a.PlantillaEstudioId).HasColumnName("IdPlantillaEstudio");

            HasRequired(a => a.PlantillaEstudio)
                .WithMany()
                .HasForeignKey(a => a.PlantillaEstudioId);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("IntegracionERPAca.PlanesERP");
        }
    }

    public class EstudioIntegracionMapping : EntityTypeConfiguration<EstudioIntegracion>
    {
        public EstudioIntegracionMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdEstudioGestor");
            Property(a => a.PlantillaEstudioIntegracionId).HasColumnName("IdRefPlanErp");
            Property(a => a.EspecializacionId).HasColumnName("IdRefEspecializacionERP");

            HasRequired(a => a.PlantillaEstudioIntegracion)
                .WithMany(a => a.EstudiosIntegracion)
                .HasForeignKey(a => a.PlantillaEstudioIntegracionId);

            HasRequired(r => r.EstudioUnir).WithOptional(t => t.EstudioIntegracion);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("IntegracionERPAca.EstudiosGestor");
        }
    }

    public class TipoEstudioUnirMapping : EntityTypeConfiguration<TipoEstudioUnir>
    {
        public TipoEstudioUnirMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdTipoEstudio");
            Property(a => a.Nombre).HasColumnName("Tipo");

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("Expedientes.TipoEstudio");
        }
    }

    public class TipoEstudioSegunUnirMapping : EntityTypeConfiguration<TipoEstudioSegunUnir>
    {
        public TipoEstudioSegunUnirMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("iCodTipoEstudioSegunUNIR");
            Property(a => a.Nombre).HasColumnName("sTipoEstudioSegunUNIR");

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("dbo.tb_UNIRGA_Cod_TiposEstudiosSegunUNIR");
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
    public class RamaUnirMapping : EntityTypeConfiguration<RamaUnir>
    {
        public RamaUnirMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdRama");
            Property(a => a.Nombre).HasColumnName("NombreRama");

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Expedientes.Rama");
        }
    }
    public class TipoAsignaturaUnirMapping : EntityTypeConfiguration<TipoAsignaturaUnir>
    {
        public TipoAsignaturaUnirMapping()
        {
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("IdTipoAsignatura");

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Expedientes.TipoAsignatura");
        }
    }
    public class PeriodoEstudioIntegracionMapping : EntityTypeConfiguration<PeriodoEstudioIntegracion>
    {
        public PeriodoEstudioIntegracionMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdPeriodoEstudioGestor");
            Property(a => a.PlantillaEstudioIntegracionId).HasColumnName("IdRefPlanERP");
            Property(a => a.PlanOfertadoId).HasColumnName("IdRefPlanOfertadoERP");
            
            HasRequired(a => a.PlantillaEstudioIntegracion)
                .WithMany(a => a.PeriodosEstudioIntegracion)
                .HasForeignKey(a => a.PlantillaEstudioIntegracionId);

            HasRequired(r => r.PeriodoEstudioUnir).WithOptional(t => t.PeriodoEstudioIntegracion);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("IntegracionERPAca.PeriodoEstudiosGestor");
        }
    }
    public class PeriodoEstudioUnirMapping : EntityTypeConfiguration<PeriodoEstudioUnir>
    {
        public PeriodoEstudioUnirMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("idUNIRGAPeriodoEstudio");
            Property(a => a.FechaInicio).HasColumnName("dtInicio");
            Property(a => a.FechaFin).HasColumnName("dtFin");
            Property(a => a.PeriodoMatriculacionId).HasColumnName("idUNIRGAPeriodoMatriculacion");
            Property(a => a.EstudioId).HasColumnName("idUNIRGAEstudios");
            Property(a => a.Borrado).HasColumnName("cBorrado");

            HasRequired(a => a.PeriodoMatriculacionUnir)
                .WithMany(a => a.PeriodosEstudioUnir)
                .HasForeignKey(a => a.PeriodoMatriculacionId);

            HasRequired(a => a.EstudioUnir)
                .WithMany(a => a.PeriodosEstudioUnir)
                .HasForeignKey(a => a.EstudioId)
                .WillCascadeOnDelete(false);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("dbo.tb_UNIRGA_PeriodosEstudios");
        }
    }

    public class PeriodoEstudioAsignaturaIntegracionMapping : EntityTypeConfiguration<PeriodoEstudioAsignaturaIntegracion>
    {
        public PeriodoEstudioAsignaturaIntegracionMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("IdPeriodoEstudioAsignaturaGestor");
            Property(a => a.AsignaturaOfertadaId).HasColumnName("IdRefAsignaturaOfertadaERP");          

            HasRequired(r => r.PeriodoEstudioAsignaturaUnir).WithOptional(t => t.PeriodoEstudioAsignaturaIntegracion);

            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            ToTable("IntegracionERPAca.PeriodoEstudioAsignaturasGestor");
        }
    }

    public class PeriodoEstudioAsignaturaUnirMapping : EntityTypeConfiguration<PeriodoEstudioAsignaturaUnir>
    {
        public PeriodoEstudioAsignaturaUnirMapping()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasColumnName("idUNIRGAPeriodoEstudioAsignatura");
            Property(a => a.PeriodoEstudioId).HasColumnName("idUNIRGAPeriodoEstudio");
            Property(a => a.AsignaturaId).HasColumnName("idUNIRGAAsignaturaEstudio");
            Property(a => a.Borrado).HasColumnName("bBorrado");

            HasRequired(a => a.PeriodoEstudioUnir)
                .WithMany(a => a.PeriodosEstudiosAsignaturasUnir)
                .HasForeignKey(a => a.PeriodoEstudioId);

            HasRequired(a => a.AsignaturaUnir)
                .WithMany(a => a.PeriodosEstudiosAsignaturasUnir)
                .HasForeignKey(a => a.AsignaturaId);

            //Llave primaria y Nombre de Tabla
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("dbo.tb_UNIRGA_PeriodosEstudiosAsignaturas");
        }
    }

    #region Oferta Academica

    public class PeriodoMatriculacionUnirMapping : EntityTypeConfiguration<PeriodoMatriculacionUnir>
    {
        public PeriodoMatriculacionUnirMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("idUNIRGAPeriodoMatriculacion");
            Property(a => a.Nombre).HasColumnName("sPeriodoMatricula");
            Property(a => a.AnyoAcademico).HasColumnName("sAnyoAcademico");
            Property(a => a.FechaInicio).HasColumnName("dtInicioPeriodoMatricula");
            Property(a => a.FechaFin).HasColumnName("dtFinPeriodoMatricula");
            Property(a => a.Nro).HasColumnName("iNumPeriodo");
            Property(a => a.Borrado).HasColumnName("cBorrado");

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("dbo.tb_UNIRGA_PeriodosMatriculacion");

            //Configuración de Llave Primaria Autonumérica.
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class PeriodoMatriculacionIntegracionMapping : EntityTypeConfiguration<PeriodoMatriculacionIntegracion>
    {
        public PeriodoMatriculacionIntegracionMapping()
        {
            //Mapeo de Propiedades y Columnas
            Property(a => a.Id).HasColumnName("IdPeriodoMatriculacionGestor");
            Property(a => a.PeriodoAcademicoId).HasColumnName("IdRefPeriodoAcademicoERP");

            HasRequired(a => a.PeriodoMatriculacion).WithOptional(a => a.PeriodoMatriculacionIntegracion);            

            //Llave primaria y Nombre de Tabla
            HasKey(a => a.Id);
            ToTable("IntegracionERPAca.PeriodosMatriculacion");
        }
    }

    #endregion

}
