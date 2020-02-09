using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Helpers
{
     /// <summary>
        /// Transport class used for output by Controllers.
        /// </summary>
        /// <typeparam name="T">The type of output data.</typeparam>
        public class DtoOutput<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DtoOutput{T}"/> class.
            /// </summary>
            /// <param name="data">Transport data.</param>
            public DtoOutput(T data)
            {
                Data = data;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DtoOutput{T}"/> class.
            /// </summary>
            /// <param name="data">Transport Data.</param>
            /// <param name="message">Status Message.</param>
            public DtoOutput(T data, string message)
            {
                Data = data;
                Message = message;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DtoOutput{T}"/> class.
            /// </summary>
            /// <param name="data">Transport data.</param>
            /// <param name="message">Status message.</param>
            /// <param name="code">Status code.</param>
            public DtoOutput(T data, string message, int code)
            {
                Data = data;
                Message = message;
                Code = code;
            }

            /// <summary>
            /// Gets or sets the status code..
            ///
            /// </summary>
            /// <value>0 for success. Use positive integer for errors.</value>
            public int Code { get; set; } = 0;

            /// <summary>
            /// Gets or sets the status message.
            /// </summary>
            /// <value>Give a message if Code is greater than 0.</value>
            public string Message { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the transport data.
            /// </summary>
            /// <value>The transport data.</value>
            public T Data { get; set; }

            /// <summary>
            /// Converts the DTO response to a JSON string.
            /// </summary>
            /// <returns>The DtoObject instance as a string.</returns>
            public string ToJsonString()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }
        }
}
