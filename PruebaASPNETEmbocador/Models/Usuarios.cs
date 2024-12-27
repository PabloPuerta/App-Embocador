namespace PruebaASPNETEmbocador.Models
{

using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Usuarios
    {

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Usuarios()
    {

        this.TurnosTrabajadoresEmbocador = new HashSet<TurnosTrabajadoresEmbocador>();

        this.TurnosHorarios = new HashSet<TurnosHorarios>();

    }


        public int IDUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; }

        public bool IsAdmin { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TurnosTrabajadoresEmbocador> TurnosTrabajadoresEmbocador { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TurnosHorarios> TurnosHorarios { get; set; }

}

}
