using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Server
{
  public class IServerList : ObservableCollection<IServer>
    {
        public IServerList() : base()
        {
            Add(new IServer("Ragnarok"));
            Add(new IServer("awsome Server"));
        }

        public void AddServer(string name)
        {
            IServer Server = new IServer(name);

            Add(Server);
            this.IndexOf(Server);
        }


        public bool UpdateServer(IServer name)
        {



            return true; 
        }
    }
}
