var indexTab = Number($("#InitialCountTabs").attr("value"));
var entityName = $("#EntiyNameForTranslation").attr("value");
var entityID=$("#ID").attr("value");
var EntityName=$("#EntityNameProperty").html();

function ChangeNameProperties() {
    $("input.name").each(function (index, obj) {
        $(obj).attr("name", entityName + ".Translations[" + index + "].Name");
    });
    $("input.description").each(function (index, obj) {
        $(obj).attr("name", entityName + ".Translations[" + index + "].Description");
    });
    $("input.EntityID").each(function(index,obj){
        var tmp=$(obj).attr("name");
        $(obj).attr("name",entityName+".Translations["+index+"].EntityID");
    });
    $("input.LanguageID").each(function(index,obj){
        var tmp=$(obj).attr("name");
        $(obj).attr("name",entityName+".Translations["+index+"].LanguageID");
    });
}
var closeTab=function () {
            var expr = /[\d]+/;
            var idContent = $(this).parents("a").attr("href").substring(1);
            var currentIndex = Number($("div.tab-pane#" + idContent + " input:first").attr("name").match(expr)[0]);

            //reajustar indices
            $("div.tab-pane input").each(function () {
                var namePropertyValue = $(this).attr("name");
                var index = Number(namePropertyValue.match(expr)[0]);
                if (index > currentIndex) {
                    index--;
                    if (/.Name/.test(namePropertyValue)) {
                        $(this).attr("name", entityName + ".Translations[" + index + "].Name");
                    }
                    else if(/.Description/.test(namePropertyValue)){
                        $(this).attr("name", entityName + ".Translations[" + index + "].Description");
                    }
                    else if (/.EntityID/.test(namePropertyValue)) {
                        $(this).attr("name", entityName + ".Translations[" + index + "].EntityID");
                    }
                    else {
                        $(this).attr("name", entityName + ".Translations[" + index + "].LanguageID");
                    }
                }
            });

            indexTab--;
            var nameLanguage = $(this).siblings("span").html().trim();
            var newLanguage = $("<a>" + nameLanguage + "</a>").addClass("new-language").attr("role", "menuitem").attr("idLanguage",$(this).parent().attr("href").substring(1)).attr("href","javascript:;");
            var newListItem = $("<li></li>").append(newLanguage);

            $("ul.dropdown-menu").append(newListItem);

            $("div.tab-pane#" + idContent).remove();
            $(this).parents("li").remove();
            $("ul.nav.nav-tabs li:first").addClass("active in")
            var idFirstTab = $("ul.nav.nav-tabs a").attr("href").substring(1);
            $("div.tab-pane#" + idFirstTab).addClass("active in");

            newLanguage.click(addLanguage);
    }
function SetUpCloseTabs() {
    $("span.close").click(closeTab);
}

function AddTab(language,idRefPanel) {
    var span = $("<span></span>", { class: "close" }).attr("aria-hidden", "true").html("&times;");
    var reference=$("<a><span>"+language+"</span></a>").attr("data-toggle","tab").attr("href","#"+idRefPanel).append(span);
    var li=$("<li></li>").append(reference);
    li.insertBefore("ul.nav.nav-tabs > li:last");
    span.click(closeTab);
}

function AddPanel(panelId,languageId) {
    var divName=$("<div></div>")
                    .addClass("form-group")
                    .append("<br/>")
                    .append($("<input/>")
                                .addClass("EntityID")
                                .attr("name",entityName+".Translations["+indexTab+"].EntityID")
                                .attr("type","hidden")
                                .attr("value",entityID))
                    .append($("<input/>")
                                .addClass("LanguageID")
                                .attr("name",entityName+".Translations["+indexTab+"].LanguageID")
                                .attr("type","hidden")
                                .attr("value",languageId))
                    .append($("<label>Name</label>").addClass("control-label col-md-2"))
                    .append($("<div></div>")
                                .addClass("col-md-10")
                                .append($("<input/>")
                                            .addClass("form-control name text-box single-line")
                                            .attr("name",entityName+".Translations["+indexTab+"].Name")
                                            .attr("type","text"))
                                .append($("<span></span>")
                                            .addClass("field-validation-valid text-danger")
                                            .attr("data-valmsg-replace","true")));

    var divDescription=$("<div></div>")
                            .addClass("form-group")
                            .append($("<label>Description</label>").addClass("control-label col-md-2"))
                            .append($("<div></div>")
                                        .addClass("col-md-10")
                                        .append($("<input/>")
                                                    .addClass("form-control name text-box single-line")
                                                    .attr("name",entityName+".Translations["+indexTab+"].Description")
                                                    .attr("type","text"))
                                        .append($("<span></span>")
                                                    .addClass("field-validation-valid text-danger")
                                                    .attr("data-valmsg-replace","true")));

    var newPane=$("<div></div>")
                    .addClass("tab-pane fade")
                    .attr("id",panelId)
                    .append(divName)
                    .append(divDescription);
        
    $("div.tab-content").append(newPane);
}
var addLanguage=function () {
        var language = $(this).html();
        var panelId = $(this).attr("id");

        AddTab(language,panelId);
        
        //find language id
        var languageId="1";
        $("#LanguagesCollection li").each(function(){
            var langName=$(this).html();
            if(langName==language.trim())
            {
                languageId=$(this).attr("idLanguage");
            }
        });

        AddPanel(panelId,languageId);

        indexTab++;
        $(this).parent().remove();
    }
function SetUpAddLanguage() {
    $("a.new-language").click(addLanguage);
}

$(document).ready(function () {
    ChangeNameProperties();
    SetUpCloseTabs();
    SetUpAddLanguage();
});