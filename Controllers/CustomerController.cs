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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("openCustomer")]
        public IActionResult OpenCustomer([FromBody] OpenCustomerRequest openCustomerRequest)
        {
            try
            {
                var result = _customerService.OpenCustomer(openCustomerRequest.AccountNo, openCustomerRequest.IdCard, openCustomerRequest.Name, openCustomerRequest.DateOfBirth, openCustomerRequest.Address, openCustomerRequest.PhoneNumber, openCustomerRequest.CardPlace, openCustomerRequest.TypeId, openCustomerRequest.UserId);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost("getAccount")]
        public IActionResult GetAccount([FromBody] AccountRequest accountRequest)
        {
            try
            {
                var result = _customerService.GetAccount(accountRequest.AccountNo, accountRequest.IdCard);

                return HelperFunctions.Instance.GetErrorResponseByError(result);
            }
            catch (Exception)
            {
                return StatusCode(500, HelperFunctions.Instance.GetErrorResponseByError());
            }
        }
    }
}
