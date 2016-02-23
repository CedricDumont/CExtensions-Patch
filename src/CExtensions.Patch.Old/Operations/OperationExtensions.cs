using CExtensions.Patch.Mapper.Patch.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch.Operations
{
    public static class OperationExtensions
    {
        // Move Operation
        public static MoveOperation Move(this PatchDocument obj, string pathToMove)
        {
            MoveOperation operation = new MoveOperation(pathToMove);
            obj.AddOperation(operation);
            return operation;
        }

        public static MoveOperation To(this MoveOperation operation, String moveToPath)
        {
            operation.Path = new Path(moveToPath);
            return operation;
        }

        //Test Operation
        public static TestOperation Test(this PatchDocument obj, string pathToMove)
        {
            TestOperation operation = new TestOperation(new Path(pathToMove));
            obj.AddOperation(operation);
            return operation;
        }

        public static void ToBe(this TestOperation operation, Object expectedResult)
        {
            operation.ExpectedResult = expectedResult;
        }

        //Add operation
        public static AddOperation Add(this PatchDocument obj, string pathToAdd)
        {
            AddOperation operation = new AddOperation(new Path(pathToAdd));
            obj.AddOperation(operation);
            return operation;
        }

        public static AppendOperation AppendTo(this PatchDocument obj, string path)
        {
            AppendOperation operation = new AppendOperation(new Path(path));
            obj.AddOperation(operation);
            return operation;
        }
        public static void ValueOf(this AppendOperation operation, String path)
        {
            operation.ValueOf = new Path(path);
        }

        //General
        public static Operation WithValue(this Operation operation, Object value)
        {
            operation.Value = value;
            return operation;
        }

        public static RemoveOperation Remove(this PatchDocument obj, string pathToMove)
        {
            RemoveOperation operation = new RemoveOperation(new Path(pathToMove));
            obj.AddOperation(operation);
            return operation;
        }

        public static Operation WithConverter(this Operation operation, ValueConverter converter)
        {
            operation.Converter = converter;
            return operation;
        }

    }
}
