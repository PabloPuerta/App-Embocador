namespace PruebaASPNETEmbocador.Models
{

using System;
    using System.Collections.Generic;
    
public partial class TurnosTrabajadoresEmbocador
{

    public int IDTurno { get; set; }

    public Nullable<int> IDUsuario { get; set; }

    public Nullable<System.DateTime> RegistroEntrada { get; set; }

    public Nullable<System.DateTime> RegistroSalida { get; set; }



    public virtual Usuarios Usuarios { get; set; }

}

}
