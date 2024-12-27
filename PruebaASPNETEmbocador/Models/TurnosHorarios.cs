namespace PruebaASPNETEmbocador.Models
{

using System;
    using System.Collections.Generic;
    
public partial class TurnosHorarios
{

    public int IDTurnoHorario { get; set; }

    public int IDUsuario { get; set; }

    public System.DateTime Fecha { get; set; }

    public System.TimeSpan HoraInicio { get; set; }

    public System.TimeSpan HoraFin { get; set; }



    public virtual Usuarios Usuarios { get; set; }
        public object Usuario { get; internal set; }
    }

}
