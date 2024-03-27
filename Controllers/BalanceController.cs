using BankSystem.Dtos.Request;
using BankSystem.Repository;
using BankSystem.Service;
using BankSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;

namespace BankSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost("getBalance")]
        public IActionResult GetBalance([FromBody] AccountRequest accountRequest)
        {
            try
            {
                var result = _balanceService.GetBalance(accountRequest.AccountNo, accountRequest.IdCard);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost("holdAmount")]
        public IActionResult HoldAmount([FromBody] HoldAmountRequest holdAmountRequest)
        {
            try
            {
                var result = _balanceService.HoldAmount(holdAmountRequest.AccountNo, holdAmountRequest.IdCard, holdAmountRequest.Amount, holdAmountRequest.Description, holdAmountRequest.ApproveBy);
                
                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost("unholdAmount")]
        public IActionResult UnholdAmount([FromBody] UnholdAmount unholdAmount)
        {
            try
            {
                var result = _balanceService.UnHoldAmount(unholdAmount.AccountNo, unholdAmount.IdCard, unholdAmount.Amount, unholdAmount.Description, unholdAmount.ApproveBy);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }
    }
}
