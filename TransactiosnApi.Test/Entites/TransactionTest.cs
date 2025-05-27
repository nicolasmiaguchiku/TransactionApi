using TransactionsApi.Models.ModelsResquets;

namespace TransactiosnApi.Test.Entites
{

    public class TransactionTest
    {
        [Fact]
        public void Constructor_GivenAllParameters_Should_Set_Properties_Correctly()
        {

            //Arange
            var Title = "Compra";
            var Amount = 100.00;
            var Date = DateTime.Now;
            var Type = TransactionType.Saida;
            var Descriptor = "Supermercado";

            //act
            var validateTransaction = new TransactionViewModel(Title, Amount, Date, Type, Descriptor);

            //Assert
            Assert.Equal(Title, validateTransaction.Title);
            Assert.Equal(Amount, validateTransaction.Amount);
            Assert.Equal(Date, validateTransaction.Date);
            Assert.Equal(Type, validateTransaction.Type);
            Assert.Equal(Descriptor, validateTransaction.Descriptor);

        }
    }
}
