using System;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Xamarin.PropertyEditing
{
	internal static class Numeric<T>
	{
		public static Func<T, T> Increment
		{
			get
			{ 
				SetupMath();
				return IncrementValue;
			}
		}

		public static Func<T, T> Decrement
		{
			get
			{
				SetupMath();
				return DecrementValue;
			}
		}

		private static Func<T, T> IncrementValue, DecrementValue;

		private static void SetupMath ()
		{
			if (IncrementValue != null)
				return;

			Task<Func<T, T>> createIncrement = Task.Run (() => {
				Type t = typeof(T);
				var add = new DynamicMethod ("Add", t, new[] { t });
				ILGenerator gen = add.GetILGenerator ();
				gen.Emit (OpCodes.Ldarg_0);

				switch (Type.GetTypeCode (t)) {
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
					gen.Emit (OpCodes.Ldc_I4_1);
					gen.Emit (OpCodes.Add_Ovf_Un);
					break;
				case TypeCode.UInt64:
					gen.Emit (OpCodes.Ldc_I8, 1L);
					gen.Emit (OpCodes.Add_Ovf_Un);
					break;
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
					gen.Emit (OpCodes.Ldc_I4_1);
					gen.Emit (OpCodes.Add_Ovf);
					break;
				case TypeCode.Int64:
					gen.Emit (OpCodes.Ldc_I8, 1L);
					gen.Emit (OpCodes.Add_Ovf);
					break;
				case TypeCode.Single:
					gen.Emit (OpCodes.Ldc_R4, 1f);
					gen.Emit (OpCodes.Add);
					break;
				case TypeCode.Double:
					gen.Emit (OpCodes.Ldc_R8, 1d);
					gen.Emit (OpCodes.Add);
					break;
				default:
					throw new NotSupportedException();
				}

				gen.Emit (OpCodes.Ret);
				return (Func<T,T>)add.CreateDelegate (typeof(Func<T, T>));
			});

			Task<Func<T, T>> createDecrement = Task.Run (() => {
				Type t = typeof(T);
				var sub = new DynamicMethod ("Sub", t, new[] { t });
				ILGenerator gen = sub.GetILGenerator ();
				gen.Emit (OpCodes.Ldarg_0);

				switch (Type.GetTypeCode (t)) {
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
					gen.Emit (OpCodes.Ldc_I4_1);
					gen.Emit (OpCodes.Sub_Ovf_Un);
					break;
				case TypeCode.UInt64:
					gen.Emit (OpCodes.Ldc_I8, 1L);
					gen.Emit (OpCodes.Sub_Ovf_Un);
					break;
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
					gen.Emit (OpCodes.Ldc_I4_1);
					gen.Emit (OpCodes.Sub_Ovf);
					break;
				case TypeCode.Int64:
					gen.Emit (OpCodes.Ldc_I8, 1L);
					gen.Emit (OpCodes.Sub_Ovf);
					break;
				case TypeCode.Single:
					gen.Emit (OpCodes.Ldc_R4, 1f);
					gen.Emit (OpCodes.Sub);
					break;
				case TypeCode.Double:
					gen.Emit (OpCodes.Ldc_R8, 1d);
					gen.Emit (OpCodes.Sub);
					break;
				default:
					throw new NotSupportedException();
				}

				gen.Emit (OpCodes.Ret);
				return (Func<T,T>)sub.CreateDelegate (typeof(Func<T, T>));
			});

			IncrementValue = createIncrement.Result;
			DecrementValue = createDecrement.Result;
		}
	}
}