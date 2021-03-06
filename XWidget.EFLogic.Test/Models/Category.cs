﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace XWidget.EFLogic.Test.Models {
    public class Category {
        public Category() {
            Children = new HashSet<Category>();
            Notes = new HashSet<Note>();
        }

        [Key]
        public virtual Guid Id { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("Children")]
        public virtual Category Parent { get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<Category> Children { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Note> Notes { get; set; }

        public bool ShouldRemoveCascadeChildren() => true;

        public bool ShouldRemoveCascadeParent() => false;
    }
}
