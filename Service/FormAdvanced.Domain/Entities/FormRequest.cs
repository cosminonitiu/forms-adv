using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Domain.Entities
{

    public record SubmittedRequest(
        string id,
        string FormName,
        string FormId,
        string ApproverUID,
        string ApproverName,
        string Owner,
        string OwnerName,
        string State,
        string Icon,
        DateTime Created,
        string Description,
        string Color,
        bool HideSections,
        SubmittedSection[] Sections
    );

    public record SubmittedSection(
        string SectionId,
        string Name,
        SubmittedRequestQuestion[] Questions
    );

    public record SubmittedRequestQuestion
    (
        string QuestionId,
        string Text,
        string Type,
        bool Required,
        string[] Options,
        string Answer,
        string[] Answers,
        int? MinAnswer,
        int? MaxAnswer,
        SubmittedRequestConditionalVisibility[] ConditionalVisibilityTriggerForOtherQuestion
    );

    public record SubmittedRequestConditionalVisibility
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