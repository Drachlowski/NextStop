namespace NextStop.Server.Mappers;

using NextStop.Domain;
using NextStop.Server.DTOs;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class StopMapper
{
    public static partial StopDto ToStopDto(this Stop stop);

    public static partial Stop ToStop(this StopForCreationDto stopDto);

    [MapperIgnoreTarget(nameof(Stop.Id))]
    public static partial void UpdateStop(this StopForUpdateDto stopDto, Stop stop);
}