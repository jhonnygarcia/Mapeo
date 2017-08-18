using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GestorMapeos.Models.Model.Erp20.Entities;

namespace GestorMapeos.Models.Model.Erp20
{
    public class ErpContext : DbContext
    {
        public ErpContext()
            : base("erp")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new EstudioMapping());
            modelBuilder.Configurations.Add(new TipoEstudioMapping());
            modelBuilder.Configurations.Add(new RamaConocimientoMapping());
            modelBuilder.Configurations.Add(new TituloMapping());
            
            modelBuilder.Configurations.Add(new PlanMapping());
            modelBuilder.Configurations.Add(new NodoMapping());
            modelBuilder.Configurations.Add(new HitoMapping());
            modelBuilder.Configurations.Add(new HitoEspecializacionMapping());
            modelBuilder.Configurations.Add(new EspecializacionMapping());

            modelBuilder.Configurations.Add(new AsignaturaMapping());
            modelBuilder.Configurations.Add(new AsignaturaPlanMapping());
            modelBuilder.Configurations.Add(new TipoAsignaturaMapping());
            modelBuilder.Configurations.Add(new DuracionPeriodoLectivoMapping());
            modelBuilder.Configurations.Add(new CursoMapping());

            //Oferta Academica
            modelBuilder.Configurations.Add(new PeriodoAcademicoMapping());
            modelBuilder.Configurations.Add(new AnyoAcademicoMapping());
            modelBuilder.Configurations.Add(new PlanOfertadoMapping());
            modelBuilder.Configurations.Add(new AsignaturaOfertadaMapping());
            modelBuilder.Configurations.Add(new PeriodoLectivoMapping());
        }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Estudio> Estudio { get; set; }
        public DbSet<TipoEstudio> TipoEstudio { get; set; }
        public DbSet<RamaConocimiento> RamaConocimiento { get; set; }
        public DbSet<Titulo> Titulo { get; set; }
        public DbSet<Nodo> Nodos { get; set; }
        public DbSet<Hito> Hitos { get; set; }
        public DbSet<HitoEspecializacion> HitosEspecializaciones { get; set; }
        public DbSet<Especializacion> Especializacion { get; set; }
        public DbSet<Asignatura> Asignatura { get; set; }
        public DbSet<AsignaturaPlan> AsignaturaPlan { get; set; }
        public DbSet<TipoAsignatura> TipoAsignatura { get; set; }
        public DbSet<DuracionPeriodoLectivo> DuracionPeriodoLectivo { get; set; }
        public DbSet<Curso> Curso { get; set; }

        //OfertaAcademica
        public DbSet<PeriodoAcademico> PeriodosAcademicos { get; set; }
        public DbSet<AnyoAcademico> AnyoAcademicos { get; set; }
        public DbSet<PlanOfertado> PlanOfertado { get; set; }
        public DbSet<AsignaturaOfertada> AsignaturaOfertada { get; set; }
        public DbSet<PeriodoLectivo> PeriodoLectivo { get; set; }

        public void SaveChangeDetach()
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var objectStateEntries = objectContext
                .ObjectStateManager
                .GetObjectStateEntries(EntityState.Added);

            SaveChanges();

            foreach (var objectStateEntry in objectStateEntries)
            {
                objectContext.Detach(objectStateEntry.Entity);
            }
        }
    }
}