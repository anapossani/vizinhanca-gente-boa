using Xunit;

namespace Vizinhanca.Tests
{
    public class SampleTests
    {
        [Fact]
        public void TesteExemplo()
        {
            // Apenas para validar que os testes estão funcionando
            int a = 2;
            int b = 3;
            int resultado = a + b;

            Assert.Equal(5, resultado);
        }
    }
}
