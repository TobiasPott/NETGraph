using System;

using NETGraph.Data;

namespace NETGraph.Core.Meta
{

    public interface IDataGenerator
    {
        DataBase Scalar(object scalar);
        DataBase List(int size, bool isResizable);
        DataBase Dict(bool isRezisable);
    }

}

