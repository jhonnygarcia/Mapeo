﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestorMapeos.Globalization.Services {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EstudioIntegracionStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EstudioIntegracionStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GestorMapeos.Globalization.Services.EstudioIntegracionStrings", typeof(EstudioIntegracionStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error de datos: los datos enviados no son validos.
        /// </summary>
        public static string ErrorIdsNoEnviados {
            get {
                return ResourceManager.GetString("ErrorIdsNoEnviados", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Esta Especialización no está asociada a este Plan de Estudio..
        /// </summary>
        public static string ErrorMapeoEspecializacionNoExistente {
            get {
                return ResourceManager.GetString("ErrorMapeoEspecializacionNoExistente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ya existe otro Mapeo para este Estudio..
        /// </summary>
        public static string ErrorMapeoExistente {
            get {
                return ResourceManager.GetString("ErrorMapeoExistente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ya existe otro Mapeo para este Plan de Estudio con esta Especialización..
        /// </summary>
        public static string ErrorMapeoPlanExistenteConEspecialización {
            get {
                return ResourceManager.GetString("ErrorMapeoPlanExistenteConEspecialización", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ya existe otro Mapeo para este Plan de Estudio sin Especialización..
        /// </summary>
        public static string ErrorMapeoPlanExistenteSinEspecializacion {
            get {
                return ResourceManager.GetString("ErrorMapeoPlanExistenteSinEspecializacion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La Plantilla Estudio a la cual está asociado este Estudio en la aplicación de Gestión de Expedientes no coincide con la definida en el Mapeo de Plantilla de Estudio que Mapea este Plan de Estudio..
        /// </summary>
        public static string ErrorMapeoPlanNoAsociadoConPlantilla {
            get {
                return ResourceManager.GetString("ErrorMapeoPlanNoAsociadoConPlantilla", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No existe un Mapeo de Plantilla de Estudio para este Plan de Estudio..
        /// </summary>
        public static string ErrorMapeoPlantillaEstudio {
            get {
                return ResourceManager.GetString("ErrorMapeoPlantillaEstudio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error de datos: No existen mapeos a eliminar con los datos proporcionados.
        /// </summary>
        public static string ErrorMapeosNoExistentes {
            get {
                return ResourceManager.GetString("ErrorMapeosNoExistentes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error de Datos: No existe el Estudio Integración..
        /// </summary>
        public static string ErrorNoEstudioIntegracion {
            get {
                return ResourceManager.GetString("ErrorNoEstudioIntegracion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No existe el Estudio en Gestor.
        /// </summary>
        public static string ErrorNoEstudioUnir {
            get {
                return ResourceManager.GetString("ErrorNoEstudioUnir", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No existe un Mapeo de Plantilla de Estudio para este Plan de Estudio..
        /// </summary>
        public static string ErrorNoPlantillaEstudio {
            get {
                return ResourceManager.GetString("ErrorNoPlantillaEstudio", resourceCulture);
            }
        }
    }
}