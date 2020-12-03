window.domFunctions = {
    blur: function (element) {
        $(element).blur();
    },
    focus: function (element) {
        $(element).focus();
    },
    select: function (element) {
        $(element).select();
    },
    click: function (element) {
        $(element).click();
    },
    scrollToBottom: function (element) {
        if (element) {
            element.scrollTop = element.scrollHeight;
        }
    },
    scrollWidth: function (element) {
        return element != null ? element.scrollWidth || 0 : 0;
    },
    scrollLeft: function (element, value) {
        if (element) $(element).animate({ scrollLeft: value }, 800);
    }
};