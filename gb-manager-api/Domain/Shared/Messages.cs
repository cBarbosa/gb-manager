namespace gb_manager.Domain.Shared
{
    public static class Messages
    {
        // SUCCESS
        public const string SUCCESS_QUERY = "Registro(s) localizado(s) com sucesso";
        public const string SUCCESS_INSERT_PERSON = "Pessoa cadastrada com sucesso";
        public const string SUCCESS_UPDATE_PERSON = "Pessoa atualizada com sucesso";
        public const string SUCCESS_AUTHENTICATION = "Usuário autenticado com sucesso";
        public const string SUCCESS_INSERT_CONTRACT = "Contrato cadastrado com sucesso";


        // ERRORs
        public const string ERROR_QUERY = "Registro(s) não localizado(s)";
        public const string ERROR_INSERT_PERSON = "Erro ao cadastrar pessoa";
        public const string ERROR_UPDATE_PERSON = "Erro ao cadastrar pessoa";
        public const string ERROR_PARAM_NOT_FOUND = "Parâmetro não encontrado";
        public const string ERROR_PERSON_ALREADY_EXISTS = "Já existe uma pessoa com o CPF informado";
        public const string ERROR_PERSON_NOT_EXISTS_RECORDID = "Não foi possível encontrar a pessoa com o ID informado";
        public const string ERROR_USER_NOT_EXISTS = "Usuário não encontrado";
        public const string ERROR_AUTHENTICATION = "Não foi possível autenticar o usuário";
        public const string ERROR_PLAN_NOT_FOUND = "Plano não encontrado";
    }
}