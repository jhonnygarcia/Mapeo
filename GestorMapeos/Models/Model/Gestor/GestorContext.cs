using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GestorMapeos.Models.Model.Gestor.Entities;

namespace GestorMapeos.Models.Model.Gestor
{
    public class GestorContext : DbContext
    {
        public GestorContext()
            : base("gestor")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new PlantillaAsignaturaMapping());
            modelBuilder.Configurations.Add(new AsignaturaPlanIntegracionMapping());
            modelBuilder.Configurations.Add(new AsignaturaUnirMapping());
            modelBuilder.Configurations.Add(new AsignaturaEstudioIntegracionMapping());

            modelBuilder.Configurations.Add(new EstudioUnirMapping());
            modelBuilder.Configurations.Add(new TipoEstudioSegunUnirMapping());
            modelBuilder.Configurations.Add(new EstudioPrincipalUnirMapping());

            modelBuilder.Configurations.Add(new PlantillaEstudioMapping());
            modelBuilder.Configurations.Add(new PlanIntegracionMapping());
            modelBuilder.Configurations.Add(new TipoEstudioUnirMapping());
            modelBuilder.Configurations.Add(new RamaUnirMapping());
            modelBuilder.Configurations.Add(new EstudioIntegracionMapping());
            modelBuilder.Configurations.Add(new TipoAsignaturaUnirMapping());

            //Oferta Academica
            modelBuilder.Configurations.Add(new PeriodoMatriculacionUnirMapping());
            modelBuilder.Configurations.Add(new PeriodoMatriculacionIntegracionMapping());
            modelBuilder.Configurations.Add(new PeriodoEstudioIntegracionMapping());
            modelBuilder.Configurations.Add(new PeriodoEstudioUnirMapping());
            modelBuilder.Configurations.Add(new PeriodoEstudioAsignaturaIntegracionMapping());
            modelBuilder.Configurations.Add(new PeriodoEstudioAsignaturaUnirMapping());
        }
        public DbSet<PlantillaAsignatura> PlantillaAsignatura { get; set; }
        public DbSet<PlantillaAsignaturaIntegracion> PlantillaAsignaturaIntegracion { get; set; }
        public DbSet<AsignaturaUnir> AsignaturaUnir { get; set; }
        public DbSet<AsignaturaIntegracion> AsignaturaIntegracion { get; set; }
        public DbSet<EstudioUnir> EstudioUnir { get; set; }
        public DbSet<TipoEstudioSegunUnir> TiposEstudiosSegunUnir { get; set; }
        public DbSet<PlantillaEstudio> PlantillaEstudio { get; set; }
        public DbSet<PlantillaEstudioIntegracion> PlantillaEstudioIntegracion { get; set; }
        public DbSet<TipoEstudioUnir> TipoEstudio { get; set; }
        public DbSet<RamaUnir> Rama { get; set; }
        public DbSet<EstudioIntegracion> EstudioIntegracion { get; set; }
        public DbSet<TipoAsignaturaUnir> TipoAsignatura { get; set; }

        //Oferta Academica
        public DbSet<PeriodoMatriculacionUnir> PeriodosMatriculacionesUnir { get; set; }
        public DbSet<PeriodoMatriculacionIntegracion> PeriodosMatriculacionesIntegracion { get; set; }
        public DbSet<PeriodoEstudioIntegracion> PeriodoEstudioIntegracion { get; set; }
        public DbSet<PeriodoEstudioUnir> PeriodoEstudioUnir { get; set; }
        public DbSet<PeriodoEstudioAsignaturaIntegracion> PeriodoEstudioAsignaturaIntegracion { get; set; }
        public DbSet<PeriodoEstudioAsignaturaUnir> PeriodoEstudioAsignaturaUnir { get; set; }

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
