using Dapper;
using System.Data;

namespace CursinhoEACH.Data;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    // Ensina o Dapper a LER do banco (DateTime -> DateOnly)
    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }

    // Ensina o Dapper a ESCREVER no banco (DateOnly -> DateTime)
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        parameter.DbType = DbType.Date;
    }
}