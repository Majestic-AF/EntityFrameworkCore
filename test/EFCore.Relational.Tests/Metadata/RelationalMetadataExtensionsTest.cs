// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Metadata
{
    public class RelationalMetadataExtensionsTest
    {
        [Fact]
        public void Can_get_and_set_fixed_length()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.Name)
                .Metadata;

            Assert.False(property.IsFixedLength());

            property.SetIsFixedLength(true);

            Assert.True(property.IsFixedLength());

            property.SetIsFixedLength(false);

            Assert.False(property.IsFixedLength());
        }

        [Fact]
        public void Can_get_and_set_index_filter()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .HasIndex(e => e.Id)
                .HasFilter("[Id] % 2 = 0")
                .Metadata;

            Assert.Equal("[Id] % 2 = 0", property.GetFilter());

            property.SetFilter("[Id] % 3 = 0");

            Assert.Equal("[Id] % 3 = 0", property.GetFilter());
        }

        [Fact]
        public void Can_get_and_set_column_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.Name)
                .Metadata;

            Assert.Equal("Name", property.GetColumnName());

            property.SetColumnName("Eman");

            Assert.Equal("Name", property.Name);
            Assert.Equal("Eman", property.GetColumnName());

            property.SetColumnName(null);

            Assert.Equal("Name", property.GetColumnName());
        }

        [Fact]
        public void Can_get_and_set_table_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            Assert.Equal("Customer", entityType.GetTableName());

            entityType.SetTableName("Customizer");

            Assert.Equal("Customer", entityType.DisplayName());
            Assert.Equal("Customizer", entityType.GetTableName());

            entityType.SetTableName(null);

            Assert.Equal("Customer", entityType.GetTableName());
        }

        [Fact]
        public void Can_get_and_set_schema_name_on_entity_type()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            Assert.Null(entityType.GetSchema());

            entityType.SetSchema("db0");

            Assert.Equal("db0", entityType.GetSchema());

            entityType.SetSchema(null);

            Assert.Null(entityType.GetSchema());
        }

        [Fact]
        public void Gets_model_schema_if_schema_on_entity_type_not_set()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            Assert.Null(entityType.GetSchema());

            modelBuilder.Model.SetDefaultSchema("db0");

            Assert.Equal("db0", entityType.GetSchema());

            modelBuilder.Model.SetDefaultSchema(null);

            Assert.Null(entityType.GetSchema());
        }

        [Fact]
        public void Can_get_and_set_column_type()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.Name)
                .Metadata;

            Assert.Null(property.GetColumnType());

            property.SetColumnType("nvarchar(max)");

            Assert.Equal("nvarchar(max)", property.GetColumnType());

            property.SetColumnType(null);

            Assert.Null(property.GetColumnType());
        }

        [Fact]
        public void Can_get_and_set_column_default_expression()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.Name)
                .Metadata;

            Assert.Null(property.GetDefaultValueSql());

            property.SetDefaultValueSql("newsequentialid()");

            Assert.Equal("newsequentialid()", property.GetDefaultValueSql());

            property.SetDefaultValueSql(null);

            Assert.Null(property.GetDefaultValueSql());
        }

        [Fact]
        public void Can_get_and_set_column_computed_expression()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.Name)
                .Metadata;

            Assert.Null(property.GetComputedColumnSql());

            property.SetComputedColumnSql("newsequentialid()");

            Assert.Equal("newsequentialid()", property.GetComputedColumnSql());

            property.SetComputedColumnSql(null);

            Assert.Null(property.GetComputedColumnSql());
        }

        [Fact]
        public void Can_get_and_set_column_default_value()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.AlternateId)
                .Metadata;

            Assert.Null(property.GetDefaultValue());

            var guid = new Guid("{3FDFC4F5-AEAB-4D72-9C96-201E004349FA}");

            property.SetDefaultValue(guid);

            Assert.Equal(guid, property.GetDefaultValue());

            property.SetDefaultValue(null);

            Assert.Null(property.GetDefaultValue());
        }

        [Fact]
        public void Can_get_and_set_column_default_value_of_enum_type()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.EnumValue)
                .Metadata;

            Assert.Null(property.GetDefaultValue());

            property.SetDefaultValue(MyEnum.Mon);

            Assert.Equal(typeof(MyEnum), property.GetDefaultValue().GetType());
            Assert.Equal(MyEnum.Mon, property.GetDefaultValue());

            property.SetDefaultValue(null);

            Assert.Null(property.GetDefaultValue());
        }

        [Fact]
        public void Throws_when_setting_column_default_value_of_wrong_type()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var property = modelBuilder
                .Entity<Customer>()
                .Property(e => e.Name)
                .Metadata;

            Assert.Null(property.GetDefaultValue());

            var guid = new Guid("{3FDFC4F5-AEAB-4D72-9C96-201E004349FA}");

            Assert.Equal(
                RelationalStrings.IncorrectDefaultValueType(
                    guid, typeof(Guid), property.Name, property.ClrType, property.DeclaringEntityType.DisplayName()),
                Assert.Throws<InvalidOperationException>(() => property.SetDefaultValue(guid)).Message);
        }

        [Fact]
        public void Can_get_and_set_column_key_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var key = modelBuilder
                .Entity<Customer>()
                .HasKey(e => e.Id)
                .Metadata;

            Assert.Equal("PK_Customer", key.GetName());

            key.SetName("PrimaryKey");

            Assert.Equal("PrimaryKey", key.GetName());

            key.SetName(null);

            Assert.Equal("PK_Customer", key.GetName());
        }

        [Fact]
        public void Can_get_and_set_column_foreign_key_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            modelBuilder
                .Entity<Customer>()
                .HasKey(e => e.Id);

            var foreignKey = modelBuilder
                .Entity<Order>()
                .HasOne<Customer>()
                .WithOne()
                .HasForeignKey<Order>(e => e.CustomerId)
                .Metadata;

            Assert.Equal("FK_Order_Customer_CustomerId", foreignKey.GetConstraintName());

            foreignKey.SetConstraintName("FK");

            Assert.Equal("FK", foreignKey.GetConstraintName());

            foreignKey.SetConstraintName(null);

            Assert.Equal("FK_Order_Customer_CustomerId", foreignKey.GetConstraintName());
        }

        [Fact]
        public void Can_get_and_set_index_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var index = modelBuilder
                .Entity<Customer>()
                .HasIndex(e => e.Id)
                .Metadata;

            Assert.Equal("IX_Customer_Id", index.GetName());

            index.SetName("MyIndex");

            Assert.Equal("MyIndex", index.GetName());

            index.SetName(null);

            Assert.Equal("IX_Customer_Id", index.GetName());
        }

        [Fact]
        public void Can_get_and_set_discriminator()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            Assert.Null(entityType.GetDiscriminatorProperty());

            var property = entityType.AddProperty("D", typeof(string));

            entityType.SetDiscriminatorProperty(property);

            Assert.Same(property, entityType.GetDiscriminatorProperty());

            entityType.SetDiscriminatorProperty(null);

            Assert.Null(entityType.GetDiscriminatorProperty());
        }

        [Fact]
        public void Setting_discriminator_on_non_root_type_throws()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;
            var property = entityType.AddProperty("D", typeof(string));

            var derivedType = modelBuilder
                .Entity<SpecialCustomer>()
                .Metadata;
            derivedType.BaseType = entityType;

            Assert.Equal(
                RelationalStrings.DiscriminatorPropertyMustBeOnRoot(nameof(SpecialCustomer)),
                Assert.Throws<InvalidOperationException>(() => derivedType.SetDiscriminatorProperty(property)).Message);
        }

        [Fact]
        public void Setting_discriminator_from_different_entity_type_throws()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            var otherType = modelBuilder
                .Entity<SpecialCustomer>()
                .Metadata;

            var property = entityType.AddProperty("D", typeof(string));

            Assert.Equal(
                RelationalStrings.DiscriminatorPropertyNotFound("D", nameof(SpecialCustomer)),
                Assert.Throws<InvalidOperationException>(() => otherType.SetDiscriminatorProperty(property)).Message);
        }

        [Fact]
        public void Can_get_and_set_discriminator_value()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            var property = entityType.AddProperty("D", typeof(string));
            entityType.SetDiscriminatorProperty(property);

            Assert.Null(entityType.GetDiscriminatorValue());

            entityType.SetDiscriminatorValue("V");

            Assert.Equal("V", entityType.GetDiscriminatorValue());

            entityType.SetDiscriminatorValue(null);

            Assert.Null(entityType.GetDiscriminatorValue());
        }

        [Fact]
        public void Setting_discriminator_value_when_discriminator_not_set_throws()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            Assert.Equal(
                RelationalStrings.NoDiscriminatorForValue("Customer", "Customer"),
                Assert.Throws<InvalidOperationException>(
                    () => entityType.SetDiscriminatorValue("V")).Message);
        }

        [Fact]
        public void Setting_incompatible_discriminator_value_throws()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());

            var entityType = modelBuilder
                .Entity<Customer>()
                .Metadata;

            var property = entityType.AddProperty("D", typeof(int));
            entityType.SetDiscriminatorProperty(property);

            Assert.Equal(
                RelationalStrings.DiscriminatorValueIncompatible("V", "D", typeof(int)),
                Assert.Throws<InvalidOperationException>(
                    () => entityType.SetDiscriminatorValue("V")).Message);

            entityType.SetDiscriminatorValue(null);
        }

        [Fact]
        public void Can_get_and_set_schema_name_on_model()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;

            Assert.Null(model.GetDefaultSchema());

            model.SetDefaultSchema("db0");

            Assert.Equal("db0", model.GetDefaultSchema());

            model.SetDefaultSchema(null);

            Assert.Null(model.GetDefaultSchema());
        }

        [Fact]
        public void Can_get_and_set_dbfunction()
        {
            var testMethod = typeof(TestDbFunctions).GetTypeInfo().GetDeclaredMethod(nameof(TestDbFunctions.MethodA));

            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;

            Assert.Null(model.FindDbFunction(testMethod));

            var dbFunc = model.AddDbFunction(testMethod);

            Assert.NotNull(dbFunc);
            Assert.Null(dbFunc.FunctionName);
            Assert.Null(dbFunc.Schema);
        }

        [Fact]
        public void Can_get_and_set_sequence()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;

            Assert.Null(model.FindSequence("Foo"));

            var sequence = model.AddSequence("Foo");

            Assert.Equal("Foo", model.FindSequence("Foo").Name);

            Assert.Equal("Foo", sequence.Name);
            Assert.Null(sequence.Schema);
            Assert.Equal(1, sequence.IncrementBy);
            Assert.Equal(1, sequence.StartValue);
            Assert.Null(sequence.MinValue);
            Assert.Null(sequence.MaxValue);
            Assert.Same(typeof(long), sequence.ClrType);

            var sequence2 = model.FindSequence("Foo");

            sequence.StartValue = 1729;
            sequence.IncrementBy = 11;
            sequence.MinValue = 2001;
            sequence.MaxValue = 2010;
            sequence.ClrType = typeof(int);

            Assert.Equal("Foo", sequence.Name);
            Assert.Null(sequence.Schema);
            Assert.Equal(11, sequence.IncrementBy);
            Assert.Equal(1729, sequence.StartValue);
            Assert.Equal(2001, sequence.MinValue);
            Assert.Equal(2010, sequence.MaxValue);
            Assert.Same(typeof(int), sequence.ClrType);

            Assert.Equal(sequence2.Name, sequence.Name);
            Assert.Equal(sequence2.Schema, sequence.Schema);
            Assert.Equal(sequence2.IncrementBy, sequence.IncrementBy);
            Assert.Equal(sequence2.StartValue, sequence.StartValue);
            Assert.Equal(sequence2.MinValue, sequence.MinValue);
            Assert.Equal(sequence2.MaxValue, sequence.MaxValue);
            Assert.Same(sequence2.ClrType, sequence.ClrType);
        }

        [Fact]
        public void Can_get_and_set_sequence_with_schema_name()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;

            Assert.Null(model.FindSequence("Foo", "Smoo"));

            var sequence = model.AddSequence("Foo", "Smoo");

            Assert.Equal("Foo", model.FindSequence("Foo", "Smoo").Name);
            Assert.Equal("Foo", model.FindSequence("Foo", "Smoo").Name);

            Assert.Equal("Foo", sequence.Name);
            Assert.Equal("Smoo", sequence.Schema);
            Assert.Equal(1, sequence.IncrementBy);
            Assert.Equal(1, sequence.StartValue);
            Assert.Null(sequence.MinValue);
            Assert.Null(sequence.MaxValue);
            Assert.Same(typeof(long), sequence.ClrType);

            var sequence2 = model.FindSequence("Foo", "Smoo");

            sequence.StartValue = 1729;
            sequence.IncrementBy = 11;
            sequence.MinValue = 2001;
            sequence.MaxValue = 2010;
            sequence.ClrType = typeof(int);

            Assert.Equal("Foo", sequence.Name);
            Assert.Equal("Smoo", sequence.Schema);
            Assert.Equal(11, sequence.IncrementBy);
            Assert.Equal(1729, sequence.StartValue);
            Assert.Equal(2001, sequence.MinValue);
            Assert.Equal(2010, sequence.MaxValue);
            Assert.Same(typeof(int), sequence.ClrType);

            Assert.Equal(sequence2.Name, sequence.Name);
            Assert.Equal(sequence2.Schema, sequence.Schema);
            Assert.Equal(sequence2.IncrementBy, sequence.IncrementBy);
            Assert.Equal(sequence2.StartValue, sequence.StartValue);
            Assert.Equal(sequence2.MinValue, sequence.MinValue);
            Assert.Equal(sequence2.MaxValue, sequence.MaxValue);
            Assert.Same(sequence2.ClrType, sequence.ClrType);
        }

        [Fact]
        public void Sequence_is_in_model_schema_if_schema_not_specified()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;
            model.SetDefaultSchema("Smoo");

            Assert.Null(model.FindSequence("Foo"));

            var sequence = model.AddSequence("Foo");

            Assert.Equal("Foo", model.FindSequence("Foo").Name);

            Assert.Equal("Foo", sequence.Name);
            Assert.Equal("Smoo", sequence.Schema);
            Assert.Equal(1, sequence.IncrementBy);
            Assert.Equal(1, sequence.StartValue);
            Assert.Null(sequence.MinValue);
            Assert.Null(sequence.MaxValue);
            Assert.Same(typeof(long), sequence.ClrType);
        }

        [Fact]
        public void Returns_same_sequence_if_schema_not_specified_explicitly()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;

            Assert.Null(model.FindSequence("Foo"));

            var sequence = model.AddSequence("Foo");

            Assert.Equal("Foo", model.FindSequence("Foo").Name);

            Assert.Equal("Foo", sequence.Name);
            Assert.Null(sequence.Schema);
            Assert.Equal(1, sequence.IncrementBy);
            Assert.Equal(1, sequence.StartValue);
            Assert.Null(sequence.MinValue);
            Assert.Null(sequence.MaxValue);
            Assert.Same(typeof(long), sequence.ClrType);

            model.SetDefaultSchema("Smoo");

            var sequence2 = model.FindSequence("Foo");

            sequence.StartValue = 1729;
            sequence.IncrementBy = 11;
            sequence.MinValue = 2001;
            sequence.MaxValue = 2010;
            sequence.ClrType = typeof(int);

            Assert.Equal("Foo", sequence.Name);
            Assert.Equal("Smoo", sequence.Schema);
            Assert.Equal(11, sequence.IncrementBy);
            Assert.Equal(1729, sequence.StartValue);
            Assert.Equal(2001, sequence.MinValue);
            Assert.Equal(2010, sequence.MaxValue);
            Assert.Same(typeof(int), sequence.ClrType);

            Assert.Equal(sequence2.Name, sequence.Name);
            Assert.Equal(sequence2.Schema, sequence.Schema);
            Assert.Equal(sequence2.IncrementBy, sequence.IncrementBy);
            Assert.Equal(sequence2.StartValue, sequence.StartValue);
            Assert.Equal(sequence2.MinValue, sequence.MinValue);
            Assert.Equal(sequence2.MaxValue, sequence.MaxValue);
            Assert.Same(sequence2.ClrType, sequence.ClrType);
        }

        [Fact]
        public void Can_get_multiple_sequences()
        {
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var model = modelBuilder.Model;

            model.AddSequence("Fibonacci");
            model.AddSequence("Golomb");

            var sequences = model.GetSequences();

            Assert.Equal(2, sequences.Count);
            Assert.Contains(sequences, s => s.Name == "Fibonacci");
            Assert.Contains(sequences, s => s.Name == "Golomb");
        }

        private enum MyEnum : byte
        {
            Son,
            Mon,
            Tue
        }

        private class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Guid AlternateId { get; set; }
            public MyEnum? EnumValue { get; set; }
        }

        private class SpecialCustomer : Customer
        {
        }

        private class Order
        {
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
        }
    }
}
