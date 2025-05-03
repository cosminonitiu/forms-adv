using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Entities
{
    public enum ConditionalVisibilityType
    {
        Equals,
        NotEquals,
        Contains,
        NotContains
    }

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        Text,
        Number,
        YesNo,
        Date,
        Dropdown,
        AD
    }


    public record FormRequest(
        string id,
        string Owner,
        string Name,
        string Icon,
        DateTime Created,
        string Description,
        string Color,
        bool HideSections,
        FormSection[] Sections
    );

    public record FormSection(
        string id,
        string Name,
        FormRequestQuestion[] Questions
    );

    public record FormRequestQuestion
    (
        string id,
        string Text,
        string Type,
        bool Required,
        string[] Options,
        int? MinAnswer,
        int? MaxAnswer,
        FormRequestConditionalVisibility[] ConditionalVisibilities
    );

    public record FormRequestConditionalVisibility
    (
        string SectionId,
        string SectionName,
        string QuestionId,
        string QuestionText,
        string Type,
        string Option,
        int NumberOption,
        string[] Options,
        int[] NumberOptions
    );
}
