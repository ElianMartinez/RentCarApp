//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentCarApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MODELOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MODELOS()
        {
            this.VEHICULOS = new HashSet<VEHICULOS>();
        }
    
        public int ID_MODELO { get; set; }
        public int ID_MARCA { get; set; }
        public string DESCRIPCION { get; set; }
        public string ESTADO { get; set; }
    
        public virtual MARCAS MARCAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEHICULOS> VEHICULOS { get; set; }
    }
}