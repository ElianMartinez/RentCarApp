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
    
    public partial class VEHICULOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VEHICULOS()
        {
            this.INSPECCIONES = new HashSet<INSPECCIONES>();
            this.RENTA = new HashSet<RENTA>();
        }
    
        public int ID_VEHICULO { get; set; }
        public string DESCRIPCION { get; set; }
        public int NO_CHASIS { get; set; }
        public int NO_MOTOR { get; set; }
        public int NO_PLACA { get; set; }
        public int ID_TIPO_VEHICULO { get; set; }
        public int ID_MARCA { get; set; }
        public int ID_MODELO { get; set; }
        public int ID_TIPO_COMNUSTIBLE { get; set; }
        public string ESTADO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSPECCIONES> INSPECCIONES { get; set; }
        public virtual MARCAS MARCAS { get; set; }
        public virtual MODELOS MODELOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RENTA> RENTA { get; set; }
        public virtual TIPOS_COMBUSTIBLES TIPOS_COMBUSTIBLES { get; set; }
        public virtual TIPOS_VEHICULOS TIPOS_VEHICULOS { get; set; }
    }
}
