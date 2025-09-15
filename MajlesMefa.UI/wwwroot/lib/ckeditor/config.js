/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
    config.language = 'fa';
    //config.uiColor = '#AADC6E';
    //config.contentsCss = 'fonts.css';
    config.contentsLangDirection = 'rtl';
    config.contentsCss = '/fonts/fonts-fa.css';
    //the next line add the new font to the combobox in CKEditor
    config.font_names = 'Yekan/"WYekan";' + config.font_names;
    //config.font_defaultLabel = 'Yekan';
    //config.filebrowserBrowseUrl = "/lib/fileman/index.html?integration=ckeditor";
    //config.filebrowserImageBrowseUrl = "/lib/fileman/index.html?integration=ckeditor&type=image";
    config.extraPlugins = 'ckawesome';
    config.fontawesomePath = '/lib/font-awesome/web-fonts-with-css/css/fontawesome-all.css';
    config.removePlugins = 'iframe';
};
