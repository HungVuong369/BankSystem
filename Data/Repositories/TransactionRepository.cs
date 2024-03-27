using BankSystem.Dtos.Response;
using BankSystem.Utilities;
using Dapper;

namespace BankSystem.Data.Repositories
{
    public class TransactionRepository
    {
        private readonly IDataAccess _dataAccess;

        public TransactionRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public ResponseDto DepositMoney(string accountNo, double amount, string description)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@accountNo", accountNo);
            parameters.Add("@amount", amount);
            parameters.Add("@description", description);

            var check = _dataAccess.ExecuteStoredProcedure<int>("depositMoney", parameters);

            return HelperFunctions.Instance.GetErrorResponseByError(check);
        }

        public ResponseDto DepositApproval(string transactionId, int status, string approveBy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@transactionId", transactionId);
            parameters.Add("@status", status);
            parameters.Add("@approveBy", approveBy);

            var check = _dataAccess.ExecuteStoredProcedure<int>("depositApproval", parameters);

            return HelperFunctions.Instance.GetErrorResponseByError(check);
        }

        public ResponseDto SellPayment(string accountNo, string idCard, string securitiesAccount, string securitiesAccountIdCard, double amount, string descriptions)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@accountNo", accountNo);
            parameters.Add("@idCard", idCard);
            parameters.Add("@securitiesAccount", securitiesAccount);
            parameters.Add("@securitiesAccountIdCard", securitiesAccountIdCard);
            parameters.Add("@amount", amount);
            parameters.Add("@descriptions", descriptions);

            var check = _dataAccess.ExecuteStoredProcedure<int>("sellPayment", parameters);
            if (check == 0)
            {
                parameters = new DynamicParameters();
                parameters.Add("@accountNum", accountNo);
                parameters.Add("@idCard", idCard);
                return new ResponseDto(_dataAccess.ExecuteStoredProcedure<BalanceTotal>("getBalance", parameters));
            }
            return HelperFunctions.Instance.GetErrorResponseByError(check);
        }

        public ResponseDto BuyPayment(string accountNo, string idCard, string securitiesAccount, string securitiesAccountIdCard, double amount, string description)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@accountNo", accountNo);
            parameters.Add("@idCard", idCard);
            parameters.Add("@securitiesAccount", securitiesAccount);
            parameters.Add("@securitiesAccountIdCard", securitiesAccountIdCard);
            parameters.Add("@amount", amount);
            parameters.Add("@description", description);

            var check = _dataAccess.ExecuteStoredProcedure<int>("buyPayment", parameters);
            if (check == 0)
            {
                parameters = new DynamicParameters();
                parameters.Add("@accountNum", accountNo);
                parameters.Add("@idCard", idCard);
                return new ResponseDto(_dataAccess.ExecuteStoredProcedure<BalanceTotal>("getBalance", parameters));
            }
            return HelperFunctions.Instance.GetErrorResponseByError(check);
        }
    }
}
