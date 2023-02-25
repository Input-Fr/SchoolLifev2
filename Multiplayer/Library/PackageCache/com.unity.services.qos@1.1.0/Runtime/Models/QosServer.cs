//-----------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by the C# SDK Code Generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Services.Qos.Http;



namespace Unity.Services.Qos.Models
{
    /// <summary>
    /// The connection information of a QoS server.
    /// </summary>
    [Preserve]
    [DataContract(Name = "QosServer")]
    internal class QosServer
    {
        /// <summary>
        /// The connection information of a QoS server.
        /// </summary>
        /// <param name="endpoints">Endpoints at which you can reach the QoS server.</param>
        /// <param name="region">The region to which the QoS server belongs.</param>
        /// <param name="services">The services using this QoS server.</param>
        [Preserve]
        public QosServer(List<string> endpoints, string region, List<string> services = default)
        {
            Endpoints = endpoints;
            Region = region;
            Services = services;
        }

        /// <summary>
        /// Endpoints at which you can reach the QoS server.
        /// </summary>
        [Preserve]
        [DataMember(Name = "endpoints", IsRequired = true, EmitDefaultValue = true)]
        public List<string> Endpoints{ get; }
        /// <summary>
        /// The region to which the QoS server belongs.
        /// </summary>
        [Preserve]
        [DataMember(Name = "region", IsRequired = true, EmitDefaultValue = true)]
        public string Region{ get; }
        /// <summary>
        /// The services using this QoS server.
        /// </summary>
        [Preserve]
        [DataMember(Name = "services", EmitDefaultValue = false)]
        public List<string> Services{ get; }
    
    }
}
