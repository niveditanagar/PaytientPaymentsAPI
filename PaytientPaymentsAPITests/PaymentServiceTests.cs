using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;
using PaytientPaymentsAPI.Services;
using System;
using System.Threading.Tasks;

namespace PaytientPaymentsAPITests
{
    [TestClass]
    public class PaymentServiceTests
    {
        [TestMethod]
        public async Task TestCreateBalanceThrowsExceptionWhenPersonDoesntExists()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            mockPersonsRepo.Setup(p => p.PersonExists(It.IsAny<int>())).Returns(Task.FromResult(false));

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            try
            {
                await paymentService.CreateBalance(65, 789.00m);
                Assert.Fail("Expected an execption when a person doesn't exist.");
            } catch (PaymentException ex)
            {
                Assert.AreEqual("This person does not exists.", ex.Message);
            }                     
        }

        [TestMethod]
        public async Task TestCreateBalanceCreatesModel()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            mockPersonsRepo.Setup(p => p.PersonExists(It.IsAny<int>())).Returns(Task.FromResult(true));

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            var paymentsModel = await paymentService.CreateBalance(12, 900.00m);
            Assert.AreEqual(12, paymentsModel.PersonId);
            Assert.AreEqual(900.00m, paymentsModel.Balance);
            Assert.AreEqual(DateTime.Now.AddDays(15).Date, paymentsModel.ScheduleDate);
        }

        [TestMethod]
        public async Task TestPostPaymentThrowsExceptionWhenPayAmountIsInvalid()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            try
            {
                await paymentService.PostPayment(0.00m, 31);
                Assert.Fail("Expected an exception when payment amount is 0.");
            } catch (PaymentException ex)
            {
                Assert.AreEqual("Payment Amount must be greater than 0.", ex.Message);
            }

            try
            {
                await paymentService.PostPayment(-50.00m, 31);
                Assert.Fail("Expected an exception when payment amount is less than 0.");
            }
            catch (PaymentException ex)
            {
                Assert.AreEqual("Payment Amount must be greater than 0.", ex.Message);
            }
        }

        [TestMethod]
        public async Task TestPostPaymentThrowsExceptionWhenUserIsInvalid()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            mockPersonsRepo.Setup(p => p.PersonExists(It.IsAny<int>())).Returns(Task.FromResult(false));

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            try
            {
                await paymentService.PostPayment(21.00m, 21);
                Assert.Fail("Expected an execption when a person doesn't exist.");
            } catch (PaymentException ex)
            {
                Assert.AreEqual("This person does not exists.", ex.Message);
            }
        }

        [TestMethod]
        public async Task TestPostPaymentThrowsExceptionWhenBalanceIsInvalid()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            mockPaymentsRepo.Setup(p => p.GetLatestPaymentAsync(It.IsAny<int>())).Returns(Task.FromResult<PaymentsModel>(null));
            mockPersonsRepo.Setup(p => p.PersonExists(It.IsAny<int>())).Returns(Task.FromResult(true));

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            try
            {
                await paymentService.PostPayment(32.00m, 32);
                Assert.Fail("Expected an exception when balance doesn't exist");
            } catch (PaymentException ex)
            {
                Assert.AreEqual("User does not have a balance.", ex.Message);
            }
        }

        [TestMethod]
        public async Task TestPostPaymentThrowsExceptionWhenBalanceIsPaid()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            var paymentModel = new PaymentsModel();

            mockPaymentsRepo.Setup(p => p.GetLatestPaymentAsync(It.IsAny<int>())).Returns(Task.FromResult<PaymentsModel>(paymentModel));
            mockPersonsRepo.Setup(p => p.PersonExists(It.IsAny<int>())).Returns(Task.FromResult(true));

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            try
            {
                await paymentService.PostPayment(6.00m, 23);
                Assert.Fail("Expected an exception when balance is paid off.");
            } catch (PaymentException ex)
            {
                Assert.AreEqual("Balance has been paid off.", ex.Message);
            }

            try
            {
                await paymentService.PostPayment(5.00m, 23);
                Assert.Fail("Expected an exception when balance is paid off.");
            }
            catch (PaymentException ex)
            {
                Assert.AreEqual("Balance has been paid off.", ex.Message);
            }
        }

        [TestMethod]
        public async Task TestPostPaymentCreatesModel()
        {
            var mockPersonsRepo = new Mock<IPersonsRepo>();
            var mockPaymentsRepo = new Mock<IPaymentsRepo>();

            var newBalance = new PaymentsModel()
            {
                Balance = 208.81m,
                PersonId = 45,
                ScheduleDate = DateTime.Now.Date
            };

            mockPaymentsRepo.Setup(p => p.GetLatestPaymentAsync(It.IsAny<int>())).Returns(Task.FromResult<PaymentsModel>(newBalance));
            mockPersonsRepo.Setup(p => p.PersonExists(It.IsAny<int>())).Returns(Task.FromResult(true));

            var paymentService = new PaymentService(mockPaymentsRepo.Object, mockPersonsRepo.Object);

            var returnedBalance = await paymentService.PostPayment(10.00m, 45);
            Assert.AreEqual(newBalance.Balance - 10.30m, returnedBalance.Balance);
            Assert.AreEqual(newBalance.PersonId, returnedBalance.PersonId);
            Assert.AreEqual(DateTime.Now.AddDays(15).Date, returnedBalance.ScheduleDate);
        }
    }
}
