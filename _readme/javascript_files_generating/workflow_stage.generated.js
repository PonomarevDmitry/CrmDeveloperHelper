if (typeof (GlobalOptionSets) == 'undefined') {
    GlobalOptionSets = { __namespace: true };
}

// ComponentType:   Attribute (2)            Count: 3
//     AttributeName           DisplayName     IsCustomizable    Behavior
//     workflow.createstage    Create Stage    True              IncludeSubcomponents
//     workflow.deletestage    Delete stage    True              IncludeSubcomponents
//     workflow.updatestage    Update Stage    True              IncludeSubcomponents
GlobalOptionSets.workflow_stageEnum = {

    'Pre_operation_20': { 'value': 20, 'text': 'Pre-operation', 'displayOrder': 1, 'name1033': 'Pre-operation', 'name1049': 'Перед основной операцией внутри транзакции' },
    'Post_operation_40': { 'value': 40, 'text': 'Post-operation', 'displayOrder': 2, 'name1033': 'Post-operation', 'name1049': 'После основной операции внутри транзакции' },
};
