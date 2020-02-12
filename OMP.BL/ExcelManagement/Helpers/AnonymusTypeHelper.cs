using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace OMP.BL.ExcelManagement.Helpers
{
    public static class AnonymousTypeHelper
    {
        #region Private readonly variables
        private static readonly ConcurrentDictionary<string, Type> GeneratedTypes = new ConcurrentDictionary<string, Type>();
        private static readonly AssemblyBuilder AssemblyBuilder;
        private static readonly ModuleBuilder ModuleBuilder;
        
        // Some objects we cache
        private static readonly CustomAttributeBuilder CompilerGeneratedAttributeBuilder = new CustomAttributeBuilder(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
        private static readonly CustomAttributeBuilder DebuggerBrowsableAttributeBuilder = new CustomAttributeBuilder(typeof(DebuggerBrowsableAttribute).GetConstructor(new[] { typeof(DebuggerBrowsableState) }), new object[] { DebuggerBrowsableState.Never });
        private static readonly CustomAttributeBuilder DebuggerHiddenAttributeBuilder = new CustomAttributeBuilder(typeof(DebuggerHiddenAttribute).GetConstructor(Type.EmptyTypes), new object[0]);

        private static readonly ConstructorInfo ObjectCtor = typeof(object).GetConstructor(Type.EmptyTypes);
        private static readonly MethodInfo ObjectToString = typeof(object).GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);

        private static readonly ConstructorInfo StringBuilderCtor = typeof(StringBuilder).GetConstructor(Type.EmptyTypes);
        private static readonly MethodInfo StringBuilderAppendString = typeof(StringBuilder).GetMethod("Append", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null);
        private static readonly MethodInfo StringBuilderAppendObject = typeof(StringBuilder).GetMethod("Append", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(object) }, null);

        private static readonly Type EqualityComparer = typeof(EqualityComparer<>);
        private static readonly Type EqualityComparerGenericArgument = EqualityComparer.GetGenericArguments()[0];
        private static readonly MethodInfo EqualityComparerDefault = EqualityComparer.GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null);
        private static readonly MethodInfo EqualityComparerEquals = EqualityComparer.GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null, new[] { EqualityComparerGenericArgument, EqualityComparerGenericArgument }, null);
        private static readonly MethodInfo EqualityComparerGetHashCode = EqualityComparer.GetMethod("GetHashCode", BindingFlags.Instance | BindingFlags.Public, null, new[] { EqualityComparerGenericArgument }, null);

        private static int Index = -1;
        #endregion Private readonly variables

        static AnonymousTypeHelper()
        {
            AppDomain myAppDomain = Thread.GetDomain();
            var assemblyName = new AssemblyName("AnonymousTypes");
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule("AnonymousTypes");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static Type CreateType(Type[] types, string[] names)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            if (names == null)
            {
                throw new ArgumentNullException("names");
            }

            if (types.Length != names.Length)
            {
                throw new ArgumentException("names");
            }

            // Anonymous classes are generics based. The generic classes
            // are distinguished by number of parameters and name of 
            // parameters. The specific types of the parameters are the 
            // generic arguments. We recreate this by creating a fullName
            // composed of all the property names, separated by a "|"
            string fullName = string.Join("|", names.Select(x => Escape(x)));

            Type type;

            if (!GeneratedTypes.TryGetValue(fullName, out type))
            {
                // We create only a single class at a time, through this lock
                // Note that this is a variant of the double-checked locking.
                // It is safe because we are using a thread safe class.
                lock (GeneratedTypes)
                {
                    if (!GeneratedTypes.TryGetValue(fullName, out type))
                    {
                        int index = Interlocked.Increment(ref Index);

                        string name = names.Length != 0 ? string.Format("<>f__AnonymousType{0}`{1}", index, names.Length) : string.Format("<>f__AnonymousType{0}", index);
                        // TypeBuilder tb = ModuleBuilder.DefineType(name, TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                        TypeBuilder tb = ModuleBuilder.DefineType(name, TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                        tb.SetCustomAttribute(CompilerGeneratedAttributeBuilder);

                        GenericTypeParameterBuilder[] generics = null;

                        if (names.Length != 0)
                        {
                            string[] genericNames = Array.ConvertAll(names, x => string.Format("<{0}>j__TPar", x));
                            generics = tb.DefineGenericParameters(genericNames);
                        }
                        else
                        {
                            generics = new GenericTypeParameterBuilder[0];
                        }

                        // .ctor
                        ConstructorBuilder constructor = tb.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, generics);
                        constructor.SetCustomAttribute(DebuggerHiddenAttributeBuilder);
                        ILGenerator ilgeneratorConstructor = constructor.GetILGenerator();
                        ilgeneratorConstructor.Emit(OpCodes.Ldarg_0);
                        ilgeneratorConstructor.Emit(OpCodes.Call, ObjectCtor);

                        var fields = new FieldBuilder[names.Length];

                        // There are two for cycles because we want to have
                        // all the getter methods before all the other 
                        // methods
                        for (int i = 0; i < names.Length; i++)
                        {
                            // field
                            fields[i] = tb.DefineField(string.Format("<{0}>i__Field", names[i]), generics[i], FieldAttributes.Private | FieldAttributes.InitOnly);
                            fields[i].SetCustomAttribute(DebuggerBrowsableAttributeBuilder);

                            // .ctor
                            constructor.DefineParameter(i + 1, ParameterAttributes.None, names[i]);
                            ilgeneratorConstructor.Emit(OpCodes.Ldarg_0);

                            if (i == 0)
                            {
                                ilgeneratorConstructor.Emit(OpCodes.Ldarg_1);
                            }
                            else if (i == 1)
                            {
                                ilgeneratorConstructor.Emit(OpCodes.Ldarg_2);
                            }
                            else if (i == 2)
                            {
                                ilgeneratorConstructor.Emit(OpCodes.Ldarg_3);
                            }
                            else if (i < 255)
                            {
                                ilgeneratorConstructor.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
                            }
                            else
                            {
                                // Ldarg uses a ushort, but the Emit only
                                // accepts short, so we use a unchecked(...),
                                // cast to short and let the CLR interpret it
                                // as ushort
                                ilgeneratorConstructor.Emit(OpCodes.Ldarg, unchecked((short)(i + 1)));
                            }

                            ilgeneratorConstructor.Emit(OpCodes.Stfld, fields[i]);

                            // getter
                            MethodBuilder getter = tb.DefineMethod(string.Format("get_{0}", names[i]), MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, CallingConventions.HasThis, generics[i], Type.EmptyTypes);
                            ILGenerator ilgeneratorGetter = getter.GetILGenerator();
                            ilgeneratorGetter.Emit(OpCodes.Ldarg_0);
                            ilgeneratorGetter.Emit(OpCodes.Ldfld, fields[i]);
                            ilgeneratorGetter.Emit(OpCodes.Ret);

                            PropertyBuilder property = tb.DefineProperty(names[i], PropertyAttributes.None, CallingConventions.HasThis, generics[i], Type.EmptyTypes);
                            property.SetGetMethod(getter);
                        }

                        // ToString()
                        MethodBuilder toString = tb.DefineMethod("ToString", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(string), Type.EmptyTypes);
                        toString.SetCustomAttribute(DebuggerHiddenAttributeBuilder);
                        ILGenerator ilgeneratorToString = toString.GetILGenerator();

                        ilgeneratorToString.DeclareLocal(typeof(StringBuilder));

                        ilgeneratorToString.Emit(OpCodes.Newobj, StringBuilderCtor);
                        ilgeneratorToString.Emit(OpCodes.Stloc_0);

                        // Equals
                        MethodBuilder equals = tb.DefineMethod("Equals", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(bool), new[] { typeof(object) });
                        equals.SetCustomAttribute(DebuggerHiddenAttributeBuilder);
                        equals.DefineParameter(1, ParameterAttributes.None, "value");
                        ILGenerator ilgeneratorEquals = equals.GetILGenerator();
                        ilgeneratorEquals.DeclareLocal(tb);

                        ilgeneratorEquals.Emit(OpCodes.Ldarg_1);
                        ilgeneratorEquals.Emit(OpCodes.Isinst, tb);
                        ilgeneratorEquals.Emit(OpCodes.Stloc_0);
                        ilgeneratorEquals.Emit(OpCodes.Ldloc_0);

                        Label equalsLabel = ilgeneratorEquals.DefineLabel();

                        // GetHashCode()
                        MethodBuilder getHashCode = tb.DefineMethod("GetHashCode", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(int), Type.EmptyTypes);
                        getHashCode.SetCustomAttribute(DebuggerHiddenAttributeBuilder);
                        ILGenerator ilgeneratorGetHashCode = getHashCode.GetILGenerator();
                        ilgeneratorGetHashCode.DeclareLocal(typeof(int));

                        if (names.Length == 0)
                        {
                            ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4_0);
                        }
                        else
                        {
                            // As done by Roslyn
                            // Note that initHash can vary, because
                            // string.GetHashCode() isn't "stable" for 
                            // different compilation of the code
                            int initHash = 0;

                            for (int i = 0; i < names.Length; i++)
                            {
                                initHash = unchecked(initHash * (-1521134295) + fields[i].Name.GetHashCode());
                            }

                            // Note that the CSC seems to generate a 
                            // different seed for every anonymous class
                            ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4, initHash);
                        }

                        for (int i = 0; i < names.Length; i++)
                        {
                            // Equals()
                            Type equalityComparerT = EqualityComparer.MakeGenericType(generics[i]);
                            MethodInfo equalityComparerTDefault = TypeBuilder.GetMethod(equalityComparerT, EqualityComparerDefault);
                            MethodInfo equalityComparerTEquals = TypeBuilder.GetMethod(equalityComparerT, EqualityComparerEquals);

                            ilgeneratorEquals.Emit(OpCodes.Brfalse_S, equalsLabel);
                            ilgeneratorEquals.Emit(OpCodes.Call, equalityComparerTDefault);
                            ilgeneratorEquals.Emit(OpCodes.Ldarg_0);
                            ilgeneratorEquals.Emit(OpCodes.Ldfld, fields[i]);
                            ilgeneratorEquals.Emit(OpCodes.Ldloc_0);
                            ilgeneratorEquals.Emit(OpCodes.Ldfld, fields[i]);
                            ilgeneratorEquals.Emit(OpCodes.Callvirt, equalityComparerTEquals);

                            // GetHashCode();
                            MethodInfo EqualityComparerTGetHashCode = TypeBuilder.GetMethod(equalityComparerT, EqualityComparerGetHashCode);

                            ilgeneratorGetHashCode.Emit(OpCodes.Stloc_0);
                            ilgeneratorGetHashCode.Emit(OpCodes.Ldc_I4, -1521134295);
                            ilgeneratorGetHashCode.Emit(OpCodes.Ldloc_0);
                            ilgeneratorGetHashCode.Emit(OpCodes.Mul);
                            ilgeneratorGetHashCode.Emit(OpCodes.Call, equalityComparerTDefault);
                            ilgeneratorGetHashCode.Emit(OpCodes.Ldarg_0);
                            ilgeneratorGetHashCode.Emit(OpCodes.Ldfld, fields[i]);
                            ilgeneratorGetHashCode.Emit(OpCodes.Callvirt, EqualityComparerTGetHashCode);
                            ilgeneratorGetHashCode.Emit(OpCodes.Add);

                            // ToString()
                            ilgeneratorToString.Emit(OpCodes.Ldloc_0);
                            ilgeneratorToString.Emit(OpCodes.Ldstr, i == 0 ? string.Format("{{ {0} = ", names[i]) : string.Format(", {0} = ", names[i]));
                            ilgeneratorToString.Emit(OpCodes.Callvirt, StringBuilderAppendString);
                            ilgeneratorToString.Emit(OpCodes.Pop);
                            ilgeneratorToString.Emit(OpCodes.Ldloc_0);
                            ilgeneratorToString.Emit(OpCodes.Ldarg_0);
                            ilgeneratorToString.Emit(OpCodes.Ldfld, fields[i]);
                            ilgeneratorToString.Emit(OpCodes.Box, generics[i]);
                            ilgeneratorToString.Emit(OpCodes.Callvirt, StringBuilderAppendObject);
                            ilgeneratorToString.Emit(OpCodes.Pop);
                        }

                        // .ctor
                        ilgeneratorConstructor.Emit(OpCodes.Ret);

                        // Equals()
                        if (names.Length == 0)
                        {
                            ilgeneratorEquals.Emit(OpCodes.Ldnull);
                            ilgeneratorEquals.Emit(OpCodes.Ceq);
                            ilgeneratorEquals.Emit(OpCodes.Ldc_I4_0);
                            ilgeneratorEquals.Emit(OpCodes.Ceq);
                        }
                        else
                        {
                            ilgeneratorEquals.Emit(OpCodes.Ret);
                            ilgeneratorEquals.MarkLabel(equalsLabel);
                            ilgeneratorEquals.Emit(OpCodes.Ldc_I4_0);
                        }

                        ilgeneratorEquals.Emit(OpCodes.Ret);

                        // GetHashCode()
                        ilgeneratorGetHashCode.Emit(OpCodes.Stloc_0);
                        ilgeneratorGetHashCode.Emit(OpCodes.Ldloc_0);
                        ilgeneratorGetHashCode.Emit(OpCodes.Ret);

                        // ToString()
                        ilgeneratorToString.Emit(OpCodes.Ldloc_0);
                        ilgeneratorToString.Emit(OpCodes.Ldstr, names.Length == 0 ? "{ }" : " }");
                        ilgeneratorToString.Emit(OpCodes.Callvirt, StringBuilderAppendString);
                        ilgeneratorToString.Emit(OpCodes.Pop);
                        ilgeneratorToString.Emit(OpCodes.Ldloc_0);
                        ilgeneratorToString.Emit(OpCodes.Callvirt, ObjectToString);
                        ilgeneratorToString.Emit(OpCodes.Ret);

                        type = tb.CreateType();

                        type = GeneratedTypes.GetOrAdd(fullName, type);
                    }
                }
            }

            if (types.Length != 0)
            {
                type = type.MakeGenericType(types);
            }

            return type;
        }

        private static string Escape(string str)
        {
            // We escape the \ with \\, so that we can safely escape the
            // "|" (that we use as a separator) with "\|"
            str = str.Replace(@"\", @"\\");
            str = str.Replace(@"|", @"\|");
            return str;
        }
    }
}