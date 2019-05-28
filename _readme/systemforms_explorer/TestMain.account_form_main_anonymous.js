
var account_form_main = (new function () {

    var Tabs = {
        'SUMMARY_TAB': {
            'Name': 'SUMMARY_TAB',
            'DefaultShowLabel': true,
            'DefaultExpanded': true,
            'DefaultVisible': true,
            'Label1033': 'Summary',
            'Sections': {
                'ACCOUNT_INFORMATION': {
                    'Name': 'ACCOUNT_INFORMATION',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'ACCOUNT INFORMATION',
                },
                'ADDRESS': {
                    'Name': 'ADDRESS',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'ADDRESS',
                },
                'MapSection': {
                    'Name': 'MapSection',
                    'DefaultShowLabel': false,
                    'DefaultVisible': true,
                    'Label1033': '',
                },
                'SOCIAL_PANE_TAB': {
                    'Name': 'SOCIAL_PANE_TAB',
                    'DefaultShowLabel': false,
                    'DefaultVisible': true,
                    'Label1033': 'SOCIAL PANE',
                },
                'Summary_section_6': {
                    'Name': 'Summary_section_6',
                    'DefaultShowLabel': false,
                    'DefaultVisible': true,
                    'Label1033': 'Assistant',
                },
                'SUMMARY_TAB_section_6': {
                    'Name': 'SUMMARY_TAB_section_6',
                    'DefaultShowLabel': false,
                    'DefaultVisible': true,
                    'Label1033': 'Section',
                },
            },
        },
        'DETAILS_TAB': {
            'Name': 'DETAILS_TAB',
            'DefaultShowLabel': true,
            'DefaultExpanded': true,
            'DefaultVisible': true,
            'Label1033': 'Details',
            'Sections': {
                'COMPANY_PROFILE': {
                    'Name': 'COMPANY_PROFILE',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'COMPANY PROFILE',
                },
                'DETAILS_TAB_section_6': {
                    'Name': 'DETAILS_TAB_section_6',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'Description',
                },
                'MARKETING': {
                    'Name': 'MARKETING',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'MARKETING',
                },
                'CONTACT_PREFERENCES': {
                    'Name': 'CONTACT_PREFERENCES',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'CONTACT PREFERENCES',
                },
                'BILLING': {
                    'Name': 'BILLING',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'BILLING',
                },
                'SHIPPING': {
                    'Name': 'SHIPPING',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'SHIPPING',
                },
                'ChildAccounts': {
                    'Name': 'ChildAccounts',
                    'DefaultShowLabel': true,
                    'DefaultVisible': true,
                    'Label1033': 'CHILD ACCOUNTS',
                },
            },
        },
    };

    var SubGrids = {
        'Contacts': 'Contacts',
        'accountopportunitiesgrid': 'accountopportunitiesgrid',
        'accountcasessgrid': 'accountcasessgrid',
        'subgrid_Entitlement': 'subgrid_Entitlement',
        'ChildAccounts': 'ChildAccounts',
    };

    var FormTypeEnum = {
        'Undefined': 0,
        'Create': 1,
        'Update': 2,
        'ReadOnly': 3,
        'Disabled': 4,
        'Bulk Edit': 6,
    };

    var RequiredLevelEnum = {
        'none': 'none',
        'required': 'required',
        'recommended': 'recommended',
    };

    var SubmitModeEnum = {
        'always': 'always',
        'never': 'never',
        'dirty': 'dirty'
    };

    var writeToConsoleInfo = function (message) {
        if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.info == 'function') {
            window.console.info(message);
        }
    };

    var writeToConsoleError = function (message) {
        if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {
            window.console.error(message);
        }
    };

    var handleError = function (e) {
        if (!e.HandledByConsole) {

            if (typeof window != 'undefined' && typeof window.console != 'undefined' && typeof window.console.error == 'function') {

                if (typeof e.name == 'string') { writeToConsoleError(e.name); }
                if (typeof e.fileName == 'string') { writeToConsoleError(e.fileName); }
                if (typeof e.lineNumber != 'undefined') { writeToConsoleError(e.lineNumber); }
                if (typeof e.message == 'string') { writeToConsoleError(e.message); }
                if (typeof e.description == 'string') { writeToConsoleError(e.description); }
                if (typeof e.stack == 'string') { writeToConsoleError(e.stack); }

                e.HandledByConsole = true;

                debugger;

                if (typeof e.message == 'string' && e.message != '') {
                    var message = e.message;
                }

                if (typeof e.description == 'string' && e.description != '') {
                    var message = e.description;
                }

                if (typeof message != 'undefined') {
                    Xrm.Utility.alertDialog('Возникла ошибка: ' + message);
                }
            }
        }
    };


    var pthis = this;

    return pthis;
}());