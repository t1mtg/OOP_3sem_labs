namespace Banks
{
    public class ClientBuilder
    {
        private Client _client = new Client();

        public ClientBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this._client = new Client();
        }

        public ClientBuilder SetName(string name)
        {
            _client.Name = name;
            return this;
        }

        public ClientBuilder SetPassport(string passport)
        {
            _client.Passport = passport;
            return this;
        }

        public ClientBuilder SetAddress(string address)
        {
            _client.Address = address;
            return this;
        }

        public Client GetClient()
        {
            Client result = _client;
            Reset();
            return result;
        }
    }
}