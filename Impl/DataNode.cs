using System;
using System.Collections.Generic;
using System.Linq;
using NETGraph.Core;
using NETGraph.Data;

namespace NETGraph.Impl.Generics
{

    public abstract class BehaviourNode : KnobNode<Guid, string>
    {

        Dictionary<string, DataBase> datas = null;
        INodeBehaviour behaviour = null;

        protected BehaviourNode(Guid id, INodeBehaviour behaviour, params DataDefinition[] dataDefinitions) : base(id)
        {
            this.behaviour = behaviour;
            if (dataDefinitions.Length > 0)
            {
                foreach (DataDefinition dataDef in dataDefinitions)
                {
                    // ToDo: Implement static mmethod to build Data<T> or Data from IDataDefinition interface (or struct type)
                    datas.Add(dataDef.Name, null);
                }
            }


        }

        public override string ToString()
        {
            return $"{string.Join(Environment.NewLine, datas)}";
        }

    }


    public interface INodeBehaviour
    {
        void Evaluate(BehaviourNode node, params BehaviourNode[] inputs);
    }

}

