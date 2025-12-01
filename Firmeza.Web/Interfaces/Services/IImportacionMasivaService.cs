using Firmeza.Web.Models.ImportacionMasiva;

namespace Firmeza.Web.Interfaces.Services;

public interface IImportacionMasivaService
{
    Task<ImportResultado> ImportarDesdeExcelAsync(Stream archivoExcel, string tipoImportacion = "Auto");
    Task<List<DatosDesnormalizados>> LeerDatosExcelAsync(Stream archivoExcel);
    Task<ImportResultado> NormalizarYGuardarDatosAsync(List<DatosDesnormalizados> datos);
    Task<byte[]> GenerarPlantillaExcelAsync(string tipoPlantilla);
}

