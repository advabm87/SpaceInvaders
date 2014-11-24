namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPlayerSettings
    {
        int Score { get; set; }

        int Lives { get; set; }

        bool Enabled { get; set; }
    }
}
