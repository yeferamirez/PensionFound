using System.ComponentModel;

namespace PensionFund.Application.Exceptions;
public enum ExceptionCodes
{
    [Description("Error al intentar conectar a la base de datos")]
    DatabaseError = 1
}
