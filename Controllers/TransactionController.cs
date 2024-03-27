using BankSystem.Dtos.Request;
using BankSystem.Repository;
using BankSystem.Service;
using BankSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("depositMoney")]
        public IActionResult DepositMoney([FromBody] DepositRequest depositRequest)
        {
            try
            {
                var result = _transactionService.DepositMoney(depositRequest.AccountNo, depositRequest.Amount, depositRequest.Description);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost("depositApproval")]
        public IActionResult DepositApproval([FromBody] DepositApprovalRequest depositApprovalRequest)
        {
            try
            {
                var result = _transactionService.DepositApproval(depositApprovalRequest.TransactionId, depositApprovalRequest.Status, depositApprovalRequest.ApproveBy);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [HttpPost("sellPayment")]
        public IActionResult SellPayment([FromBody] SellPaymentRequest sellPaymentRequest)
        {
            try
            {
                var result = _transactionService.SellPayment(sellPaymentRequest.AccountNo, sellPaymentRequest.IdCard, sellPaymentRequest.SecuritiesAccount, sellPaymentRequest.SecuritiesAccountIdCard, sellPaymentRequest.Amount, sellPaymentRequest.Descriptions);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [HttpPost("buyPayment")]
        public IActionResult BuyPayment([FromBody] BuyPaymentRequest buyPaymentRequest)
        {
            try
            {
                var result = _transactionService.BuyPayment(buyPaymentRequest.AccountNo, buyPaymentRequest.IdCard, buyPaymentRequest.SecuritiesAccount, buyPaymentRequest.SecuritiesAccountIdCard, buyPaymentRequest.Amount, buyPaymentRequest.Description);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }
    }
}
