namespace MedAtlas.Application.Features.Library.Common;

public static class ValidadorPdf
{
    private static readonly byte[] PdfHeaderBytes = [0x25, 0x50, 0x44, 0x46];

    public static bool ValidarAssinaturaPdf(Stream fluxoArquivo)
    {
        if (fluxoArquivo == null || fluxoArquivo.Length < PdfHeaderBytes.Length)
            return false;

        byte[] cabecalho = new byte[PdfHeaderBytes.Length];

        long posicaoOriginal = fluxoArquivo.Position;

        fluxoArquivo.Position = 0;
        int bytesLidos = fluxoArquivo.Read(cabecalho, 0, PdfHeaderBytes.Length);
        fluxoArquivo.Position = posicaoOriginal;

        if (bytesLidos < PdfHeaderBytes.Length)
            return false;

        for (int i = 0; i < PdfHeaderBytes.Length; i++)
        {
            if (cabecalho[i] != PdfHeaderBytes[i])
                return false;
        }

        return true;
    }
}