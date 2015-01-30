using System;
using System.ServiceModel.Configuration;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// app.configに登録できるようにするためのエレメント
    /// </summary>
    //************************************************************************
    public class CustomElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(CustomEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new CustomEndpointBehavior();
        }
    }
}
