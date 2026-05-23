# -*- coding: utf-8 -*-
"""
VB.NET商业程序加密授权框架 - 技术文档 PDF 生成脚本
"""
import os
from reportlab.lib.pagesizes import A4
from reportlab.lib.units import inch, cm
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.enums import TA_LEFT, TA_CENTER, TA_JUSTIFY
from reportlab.lib import colors
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle,
    PageBreak, KeepTogether, Image, CondPageBreak
)
from reportlab.platypus.tableofcontents import TableOfContents
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.pdfbase.pdfmetrics import registerFontFamily
import hashlib

# ━━ Color Palette ━━
ACCENT       = colors.HexColor('#217590')
TEXT_PRIMARY  = colors.HexColor('#202224')
TEXT_MUTED    = colors.HexColor('#848a8f')
BG_SURFACE   = colors.HexColor('#d4d9de')
BG_PAGE      = colors.HexColor('#eaedef')
TABLE_HEADER_COLOR = ACCENT
TABLE_HEADER_TEXT  = colors.white
TABLE_ROW_EVEN     = colors.white
TABLE_ROW_ODD      = BG_SURFACE

# ━━ Font Registration ━━
pdfmetrics.registerFont(TTFont('SarasaSC', '/usr/share/fonts/truetype/chinese/SarasaMonoSC-Regular.ttf'))
pdfmetrics.registerFont(TTFont('SarasaSCBold', '/usr/share/fonts/truetype/chinese/SarasaMonoSC-Bold.ttf'))
pdfmetrics.registerFont(TTFont('SarasaSCSemiBold', '/usr/share/fonts/truetype/chinese/SarasaMonoSC-SemiBold.ttf'))

registerFontFamily('SarasaSC', normal='SarasaSC', bold='SarasaSCBold')

# ━━ Page Setup ━━
PAGE_W, PAGE_H = A4
LEFT_MARGIN = 1.0 * inch
RIGHT_MARGIN = 1.0 * inch
TOP_MARGIN = 0.8 * inch
BOTTOM_MARGIN = 0.8 * inch
CONTENT_W = PAGE_W - LEFT_MARGIN - RIGHT_MARGIN

# ━━ Styles ━━
styles = getSampleStyleSheet()

cover_title_style = ParagraphStyle(
    'CoverTitle', fontName='SarasaSCBold', fontSize=28, leading=40,
    alignment=TA_CENTER, textColor=ACCENT, spaceAfter=12
)
cover_subtitle_style = ParagraphStyle(
    'CoverSubtitle', fontName='SarasaSC', fontSize=16, leading=24,
    alignment=TA_CENTER, textColor=TEXT_MUTED, spaceAfter=8
)
cover_meta_style = ParagraphStyle(
    'CoverMeta', fontName='SarasaSC', fontSize=11, leading=18,
    alignment=TA_CENTER, textColor=TEXT_MUTED
)

h1_style = ParagraphStyle(
    'H1', fontName='SarasaSC', fontSize=20, leading=28,
    textColor=ACCENT, spaceBefore=18, spaceAfter=12, wordWrap='CJK'
)
h2_style = ParagraphStyle(
    'H2', fontName='SarasaSC', fontSize=16, leading=22,
    textColor=ACCENT, spaceBefore=14, spaceAfter=8, wordWrap='CJK'
)
h3_style = ParagraphStyle(
    'H3', fontName='SarasaSC', fontSize=13, leading=18,
    textColor=TEXT_PRIMARY, spaceBefore=10, spaceAfter=6, wordWrap='CJK'
)
body_style = ParagraphStyle(
    'Body', fontName='SarasaSC', fontSize=10.5, leading=18,
    alignment=TA_LEFT, textColor=TEXT_PRIMARY, spaceAfter=6,
    firstLineIndent=21, wordWrap='CJK'
)
body_no_indent = ParagraphStyle(
    'BodyNoIndent', fontName='SarasaSC', fontSize=10.5, leading=18,
    alignment=TA_LEFT, textColor=TEXT_PRIMARY, spaceAfter=6, wordWrap='CJK'
)
code_style = ParagraphStyle(
    'Code', fontName='SarasaSC', fontSize=8.5, leading=12,
    alignment=TA_LEFT, textColor=colors.HexColor('#1a1a2e'),
    backColor=colors.HexColor('#f0f4f8'),
    leftIndent=12, rightIndent=12, spaceBefore=4, spaceAfter=4,
    wordWrap='CJK'
)
bullet_style = ParagraphStyle(
    'Bullet', fontName='SarasaSC', fontSize=10.5, leading=18,
    alignment=TA_LEFT, textColor=TEXT_PRIMARY, spaceAfter=4,
    leftIndent=24, bulletIndent=12, wordWrap='CJK'
)
caption_style = ParagraphStyle(
    'Caption', fontName='SarasaSC', fontSize=9, leading=14,
    alignment=TA_CENTER, textColor=TEXT_MUTED, spaceBefore=3, spaceAfter=6
)
toc_h1_style = ParagraphStyle(
    'TOCH1', fontName='SarasaSC', fontSize=13, leftIndent=20,
    spaceBefore=4, spaceAfter=2
)
toc_h2_style = ParagraphStyle(
    'TOCH2', fontName='SarasaSC', fontSize=11, leftIndent=40,
    spaceBefore=2, spaceAfter=2
)

header_cell_style = ParagraphStyle(
    'HeaderCell', fontName='SarasaSC', fontSize=10, leading=14,
    alignment=TA_CENTER, textColor=colors.white, wordWrap='CJK'
)
cell_style = ParagraphStyle(
    'Cell', fontName='SarasaSC', fontSize=9.5, leading=14,
    alignment=TA_LEFT, textColor=TEXT_PRIMARY, wordWrap='CJK'
)
cell_center_style = ParagraphStyle(
    'CellCenter', fontName='SarasaSC', fontSize=9.5, leading=14,
    alignment=TA_CENTER, textColor=TEXT_PRIMARY, wordWrap='CJK'
)

# ━━ TocDocTemplate ━━
class TocDocTemplate(SimpleDocTemplate):
    def afterFlowable(self, flowable):
        if hasattr(flowable, 'bookmark_name'):
            level = getattr(flowable, 'bookmark_level', 0)
            text = getattr(flowable, 'bookmark_text', '')
            key = getattr(flowable, 'bookmark_key', '')
            self.notify('TOCEntry', (level, text, self.page, key))

def add_heading(text, style, level=0):
    key = 'h_%s' % hashlib.md5(text.encode('utf-8')).hexdigest()[:8]
    p = Paragraph('<a name="%s"/>%s' % (key, text), style)
    p.bookmark_name = text
    p.bookmark_level = level
    p.bookmark_text = text
    p.bookmark_key = key
    return p

def make_table(data, col_widths=None):
    if col_widths is None:
        col_widths = [CONTENT_W / len(data[0])] * len(data[0])
    t = Table(data, colWidths=col_widths, hAlign='CENTER')
    style_cmds = [
        ('BACKGROUND', (0, 0), (-1, 0), TABLE_HEADER_COLOR),
        ('TEXTCOLOR', (0, 0), (-1, 0), TABLE_HEADER_TEXT),
        ('GRID', (0, 0), (-1, -1), 0.5, TEXT_MUTED),
        ('VALIGN', (0, 0), (-1, -1), 'MIDDLE'),
        ('LEFTPADDING', (0, 0), (-1, -1), 8),
        ('RIGHTPADDING', (0, 0), (-1, -1), 8),
        ('TOPPADDING', (0, 0), (-1, -1), 5),
        ('BOTTOMPADDING', (0, 0), (-1, -1), 5),
    ]
    for i in range(1, len(data)):
        bg = TABLE_ROW_EVEN if i % 2 == 1 else TABLE_ROW_ODD
        style_cmds.append(('BACKGROUND', (0, i), (-1, i), bg))
    t.setStyle(TableStyle(style_cmds))
    return t

def code_block(text):
    lines = text.strip().split('\n')
    formatted = '<br/>'.join(lines)
    return Paragraph(formatted, code_style)

# ━━ Build Document ━━
output_path = '/home/z/my-project/download/VBNetLicenseFramework/VBNetLicenseFramework_TechnicalDoc.pdf'

doc = TocDocTemplate(
    output_path,
    pagesize=A4,
    leftMargin=LEFT_MARGIN,
    rightMargin=RIGHT_MARGIN,
    topMargin=TOP_MARGIN,
    bottomMargin=BOTTOM_MARGIN,
    title='VB.NET商业程序加密授权框架技术文档',
    author='Z.ai',
    creator='Z.ai'
)

story = []

# ━━ Cover Page ━━
story.append(Spacer(1, 120))
story.append(Paragraph('<b>VB.NET商业程序加密授权框架</b>', cover_title_style))
story.append(Spacer(1, 16))
story.append(Paragraph('技术设计文档', cover_subtitle_style))
story.append(Spacer(1, 40))

# Decorative line
line_data = [['']]
line_table = Table(line_data, colWidths=[CONTENT_W * 0.5], rowHeights=[2])
line_table.setStyle(TableStyle([
    ('BACKGROUND', (0, 0), (-1, -1), ACCENT),
    ('LINEBELOW', (0, 0), (-1, -1), 0, colors.white),
]))
story.append(line_table)

story.append(Spacer(1, 40))
story.append(Paragraph('支持离线授权与在线授权双模式', cover_meta_style))
story.append(Spacer(1, 8))
story.append(Paragraph('RSA-2048数字签名 / SHA-256硬件指纹 / HMAC-SHA256请求验证', cover_meta_style))
story.append(Spacer(1, 30))
story.append(Paragraph('版本: 1.0', cover_meta_style))
story.append(Paragraph('日期: 2026年5月', cover_meta_style))
story.append(PageBreak())

# ━━ Table of Contents ━━
story.append(Paragraph('<b>目录</b>', h1_style))
story.append(Spacer(1, 12))
toc = TableOfContents()
toc.levelStyles = [toc_h1_style, toc_h2_style]
story.append(toc)
story.append(PageBreak())

# ━━ Chapter 1: System Overview ━━
story.append(add_heading('<b>一、系统概述</b>', h1_style, level=0))
story.append(Paragraph(
    '本框架是一套专为VB.NET WinForms商业应用程序设计的加密授权系统，旨在防止程序的非法拷贝和未授权运行。'
    '系统采用RSA-2048非对称加密算法进行许可证签名与验证，SHA-256哈希算法进行硬件指纹生成与脱敏，'
    'HMAC-SHA256算法进行在线请求的完整性校验，构建了一套完整的、安全可靠的软件授权保护体系。', body_style))
story.append(Paragraph(
    '该框架的核心设计理念是"先离线后在线"的验证策略。程序每次启动时，首先尝试从本地缓存加载并验证许可证（离线模式），'
    '如果离线验证失败，则自动尝试在线授权。在线授权成功后，许可证会被自动缓存到本地，'
    '使得下次启动时可以直接通过离线模式完成验证，无需再次联网。这种设计既保证了离线环境下的可用性，'
    '又充分利用了在线授权的便捷性，为最终用户提供了流畅的使用体验。', body_style))
story.append(Paragraph(
    '在安全性方面，系统采用了多层次的防护措施。首先，硬件指纹通过SHA-256单向哈希进行脱敏处理，'
    '客户发送给厂商的只是不可逆的哈希摘要，不会泄露真实的硬件信息。其次，许可证数据使用RSA-2048私钥进行数字签名，'
    '没有私钥的攻击者无法伪造有效的许可证。再次，在线授权请求使用HMAC-SHA256进行签名验证，'
    '配合时间戳机制有效防止请求伪造和重放攻击。最后，许可证与硬件指纹绑定，确保一个许可证只能在特定机器上使用，'
    '有效防止了许可证的非法转移和共享。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>1.1 核心特性</b>', h2_style, level=1))

features_data = [
    [Paragraph('<b>特性</b>', header_cell_style), Paragraph('<b>说明</b>', header_cell_style)],
    [Paragraph('双模式授权', cell_style), Paragraph('支持离线授权和在线授权两种模式，离线优先、在线兜底', cell_style)],
    [Paragraph('硬件指纹绑定', cell_style), Paragraph('采集CPU、主板、MAC地址等多维硬件信息，生成唯一指纹哈希', cell_style)],
    [Paragraph('数据脱敏', cell_style), Paragraph('硬件信息经SHA-256哈希后传输，原始硬件数据不离开客户机器', cell_style)],
    [Paragraph('RSA-2048签名', cell_style), Paragraph('许可证使用RSA-2048私钥签名，公钥验证，不可伪造', cell_style)],
    [Paragraph('HMAC请求验证', cell_style), Paragraph('在线请求使用HMAC-SHA256签名，防止请求伪造和重放攻击', cell_style)],
    [Paragraph('许可证缓存', cell_style), Paragraph('在线授权成功后自动缓存许可证，下次启动可离线验证', cell_style)],
    [Paragraph('可视化工具', cell_style), Paragraph('提供厂商端密钥生成工具和许可证生成工具的WinForms界面', cell_style)],
]
story.append(Spacer(1, 6))
story.append(make_table(features_data, [CONTENT_W * 0.25, CONTENT_W * 0.75]))
story.append(Spacer(1, 6))

# ━━ Chapter 2: Architecture ━━
story.append(add_heading('<b>二、系统架构设计</b>', h1_style, level=0))
story.append(Paragraph(
    '本系统采用分层模块化架构设计，将整个授权流程拆分为多个职责清晰的组件。客户端和服务端共享部分核心模块（如加密工具和数据模型），'
    '各自又有独立的业务逻辑模块。这种设计使得系统具有良好的可维护性和可扩展性，'
    '开发者可以根据实际需求灵活地替换或增强特定模块。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>2.1 模块组成</b>', h2_style, level=1))

modules_data = [
    [Paragraph('<b>模块</b>', header_cell_style), Paragraph('<b>所属端</b>', header_cell_style),
     Paragraph('<b>文件名</b>', header_cell_style), Paragraph('<b>职责</b>', header_cell_style)],
    [Paragraph('LicenseData', cell_style), Paragraph('共享', cell_center_style),
     Paragraph('LicenseData.vb', cell_style), Paragraph('许可证数据模型、枚举、请求/响应结构', cell_style)],
    [Paragraph('CryptoHelper', cell_style), Paragraph('共享', cell_center_style),
     Paragraph('CryptoHelper.vb', cell_style), Paragraph('RSA签名/验证、AES加解密、HMAC、SHA-256', cell_style)],
    [Paragraph('HardwareInfoCollector', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('HardwareInfoCollector.vb', cell_style), Paragraph('WMI采集CPU/主板/MAC/BIOS/磁盘信息', cell_style)],
    [Paragraph('FingerprintGenerator', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('FingerprintGenerator.vb', cell_style), Paragraph('硬件信息拼接、SHA-256哈希、脱敏导出', cell_style)],
    [Paragraph('LicenseValidator', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('LicenseValidator.vb', cell_style), Paragraph('许可证签名验证、指纹比对、过期检查', cell_style)],
    [Paragraph('OfflineLicenseProvider', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('OfflineLicenseProvider.vb', cell_style), Paragraph('离线授权：导出指纹、导入许可证、缓存管理', cell_style)],
    [Paragraph('OnlineLicenseProvider', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('OnlineLicenseProvider.vb', cell_style), Paragraph('在线授权：HTTP请求、HMAC签名、响应验证', cell_style)],
    [Paragraph('LicenseManager', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('LicenseManager.vb', cell_style), Paragraph('统一入口：先离线后在线验证策略', cell_style)],
    [Paragraph('LicenseDialog', cell_style), Paragraph('客户端', cell_center_style),
     Paragraph('LicenseDialog.vb', cell_style), Paragraph('授权管理WinForms对话框', cell_style)],
    [Paragraph('KeyGenerator', cell_style), Paragraph('厂商端', cell_center_style),
     Paragraph('KeyGenerator.vb', cell_style), Paragraph('RSA密钥对生成工具', cell_style)],
    [Paragraph('LicenseGenerator', cell_style), Paragraph('厂商端', cell_center_style),
     Paragraph('LicenseGenerator.vb', cell_style), Paragraph('许可证生成、RSA签名、文件输出', cell_style)],
    [Paragraph('LicenseGeneratorTool', cell_style), Paragraph('厂商端', cell_center_style),
     Paragraph('LicenseGeneratorTool.vb', cell_style), Paragraph('许可证生成WinForms工具', cell_style)],
    [Paragraph('LicenseServerController', cell_style), Paragraph('服务端', cell_center_style),
     Paragraph('LicenseServerController.vb', cell_style), Paragraph('ASP.NET Web API在线授权控制器', cell_style)],
]
story.append(Spacer(1, 6))
story.append(make_table(modules_data, [CONTENT_W*0.18, CONTENT_W*0.10, CONTENT_W*0.28, CONTENT_W*0.44]))
story.append(Spacer(1, 6))

story.append(Spacer(1, 12))
story.append(add_heading('<b>2.2 加密体系设计</b>', h2_style, level=1))
story.append(Paragraph(
    '本系统构建了三层加密防护体系，分别保障数据脱敏、许可证不可伪造和通信安全三个安全目标。'
    '第一层是SHA-256哈希脱敏层，用于将原始硬件信息转换为不可逆的哈希摘要，确保客户的真实硬件数据不会泄露。'
    '第二层是RSA-2048数字签名层，厂商使用私钥对许可证数据进行签名，客户端使用内嵌的公钥进行验证，'
    '确保许可证数据的完整性和不可伪造性。第三层是HMAC-SHA256通信验证层，用于在线授权场景下验证请求的合法性，'
    '防止请求伪造和重放攻击。', body_style))

crypto_data = [
    [Paragraph('<b>加密算法</b>', header_cell_style), Paragraph('<b>密钥长度</b>', header_cell_style),
     Paragraph('<b>用途</b>', header_cell_style), Paragraph('<b>使用场景</b>', header_cell_style)],
    [Paragraph('SHA-256', cell_style), Paragraph('256位', cell_center_style),
     Paragraph('单向哈希', cell_style), Paragraph('硬件指纹生成与脱敏', cell_style)],
    [Paragraph('RSA', cell_style), Paragraph('2048位', cell_center_style),
     Paragraph('数字签名/验证', cell_style), Paragraph('许可证签名（私钥）与验证（公钥）', cell_style)],
    [Paragraph('AES-256', cell_style), Paragraph('256位', cell_center_style),
     Paragraph('对称加解密', cell_style), Paragraph('许可证数据加密传输（可选）', cell_style)],
    [Paragraph('HMAC-SHA256', cell_style), Paragraph('256位', cell_center_style),
     Paragraph('消息认证码', cell_style), Paragraph('在线请求完整性校验', cell_style)],
]
story.append(Spacer(1, 6))
story.append(make_table(crypto_data, [CONTENT_W*0.18, CONTENT_W*0.14, CONTENT_W*0.20, CONTENT_W*0.48]))
story.append(Spacer(1, 6))

# ━━ Chapter 3: Offline Authorization ━━
story.append(add_heading('<b>三、离线授权流程</b>', h1_style, level=0))
story.append(Paragraph(
    '离线授权适用于无法连接互联网或网络环境受限的客户场景。整个流程通过文件交换的方式完成授权，'
    '客户将硬件指纹文件发送给厂商，厂商生成许可证文件后返回给客户，客户导入许可证即可完成授权。'
    '该流程完全不需要网络连接，适用于严格的内网隔离环境。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>3.1 流程步骤</b>', h2_style, level=1))

steps = [
    ('步骤一：采集硬件信息', '程序启动后，HardwareInfoCollector通过WMI（Windows Management Instrumentation）接口采集本机的硬件特征信息，'
     '主要包括CPU处理器ID（Win32_Processor.ProcessorId）、主板序列号（Win32_BaseBoard.SerialNumber）、'
     '网卡MAC地址（Win32_NetworkAdapter.MACAddress，过滤虚拟网卡）、BIOS序列号（Win32_BIOS.SerialNumber）'
     '以及系统盘序列号（Win32_DiskDrive.SerialNumber）。这些信息组合后可以唯一标识一台计算机。'),
    ('步骤二：生成硬件指纹哈希', 'FingerprintGenerator将采集到的硬件信息按照固定顺序和分隔符拼接为原始字符串，'
     '然后使用SHA-256算法计算哈希摘要。哈希过程是单向不可逆的，无法从64字符的十六进制哈希值反推出原始硬件信息，'
     '实现了数据脱敏。相同的硬件信息始终产生相同的哈希值，不同的硬件信息产生完全不同的哈希值。'),
    ('步骤三：导出指纹请求文件', '客户端将硬件指纹哈希导出为标准格式的TXT文件，文件包含产品名称、产品版本、'
     '导出时间、硬件指纹哈希值等信息。客户将该文件通过邮件、U盘等方式发送给软件厂商。'),
    ('步骤四：厂商生成许可证', '厂商收到指纹文件后，使用LicenseGeneratorTool导入指纹文件，设置授权参数'
     '（许可证类型、有效期、客户名称等），工具使用RSA私钥对许可证数据进行数字签名，生成许可证文件（.lic）。'
     '许可证格式为：Base64(JSON数据) + "." + Base64(RSA签名)。'),
    ('步骤五：客户导入许可证', '客户收到许可证文件后，通过授权对话框导入。程序使用内嵌的RSA公钥验证签名完整性，'
     '然后比对许可证中的硬件指纹与当前机器的硬件指纹是否匹配，最后检查许可证是否过期。'
     '验证通过后，许可证被缓存到本地AppData目录，下次启动可直接离线验证。'),
]
for title, desc in steps:
    story.append(Paragraph('<b>%s</b>' % title, h3_style))
    story.append(Paragraph(desc, body_style))

# ━━ Chapter 4: Online Authorization ━━
story.append(add_heading('<b>四、在线授权流程</b>', h1_style, level=0))
story.append(Paragraph(
    '在线授权适用于可以连接互联网的客户场景，实现了全自动的授权激活过程。客户只需点击"在线激活"按钮，'
    '程序即可自动完成硬件信息采集、请求构建、服务器通信、许可证验证和本地缓存的全流程，'
    '无需手动交换文件，极大地简化了授权操作。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>4.1 请求安全机制</b>', h2_style, level=1))
story.append(Paragraph(
    '在线授权请求采用多重安全机制保障通信安全。首先，每个请求都包含一个精确到毫秒的时间戳，'
    '服务端会验证时间戳与服务器时间的偏差是否在5分钟以内，超过有效期的请求将被拒绝，'
    '有效防止了请求的重放攻击。其次，每个请求都携带HMAC-SHA256签名，'
    '签名的内容包括硬件指纹、产品名称、产品版本和时间戳，使用客户端与服务端共享的密钥计算。'
    '服务端收到请求后会重新计算HMAC并与请求中的签名比对，如果不一致则拒绝请求，'
    '有效防止了请求内容的篡改和伪造。最后，生产环境必须使用HTTPS协议传输，'
    '确保通信数据在传输过程中不被窃听或篡改。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>4.2 服务端处理逻辑</b>', h2_style, level=1))
story.append(Paragraph(
    '服务端收到在线授权请求后，执行以下处理步骤：第一步，验证HMAC签名，确保请求未被篡改；'
    '第二步，验证时间戳，确保请求在有效期内；第三步，查询授权数据库，检查该硬件指纹是否已注册授权；'
    '第四步，如果已注册，生成包含完整授权信息的LicenseData对象；第五步，使用RSA私钥对许可证数据进行数字签名；'
    '第六步，将签名许可证封装为OnlineLicenseResponse返回给客户端。整个处理过程在服务端自动完成，'
    '无需人工干预，实现了授权的自动化管理。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>4.3 许可证自动缓存</b>', h2_style, level=1))
story.append(Paragraph(
    '在线授权成功后，客户端会将验证通过的签名许可证字符串自动缓存到本地文件系统。'
    '缓存目录默认位于用户的AppData目录下，文件名为license.dat。'
    '下次程序启动时，LicenseManager会首先尝试从本地缓存加载许可证进行离线验证，'
    '只有当离线验证失败时才会再次尝试在线授权。这种"在线授权一次，离线验证多次"的机制，'
    '既保证了授权的便捷性，又减少了对服务器的依赖，即使在网络不稳定或临时断网的情况下，'
    '已授权的软件仍然可以正常使用。', body_style))

# ━━ Chapter 5: Startup Verification ━━
story.append(add_heading('<b>五、启动验证策略</b>', h1_style, level=0))
story.append(Paragraph(
    '程序每次启动时，LicenseManager按照"先离线后在线"的策略执行授权验证。'
    '该策略的核心思想是：优先使用本地缓存的许可证进行离线验证，只有在离线验证失败时才尝试在线授权。'
    '这种设计最大化了离线可用性，最小化了对网络连接的依赖。', body_style))

verify_data = [
    [Paragraph('<b>步骤</b>', header_cell_style), Paragraph('<b>操作</b>', header_cell_style),
     Paragraph('<b>成功条件</b>', header_cell_style), Paragraph('<b>失败处理</b>', header_cell_style)],
    [Paragraph('1', cell_center_style), Paragraph('检查本地缓存许可证', cell_style),
     Paragraph('缓存文件存在', cell_style), Paragraph('跳至步骤4', cell_style)],
    [Paragraph('2', cell_center_style), Paragraph('验证许可证签名', cell_style),
     Paragraph('RSA公钥验证通过', cell_style), Paragraph('跳至步骤4', cell_style)],
    [Paragraph('3', cell_center_style), Paragraph('比对硬件指纹', cell_style),
     Paragraph('与当前机器匹配', cell_style), Paragraph('跳至步骤4', cell_style)],
    [Paragraph('4', cell_center_style), Paragraph('检查过期时间', cell_style),
     Paragraph('未过期', cell_style), Paragraph('跳至步骤5', cell_style)],
    [Paragraph('5', cell_center_style), Paragraph('在线授权请求', cell_style),
     Paragraph('服务器返回有效许可证', cell_style), Paragraph('显示授权对话框', cell_style)],
    [Paragraph('6', cell_center_style), Paragraph('缓存许可证', cell_style),
     Paragraph('写入本地文件', cell_style), Paragraph('授权完成', cell_style)],
]
story.append(Spacer(1, 6))
story.append(make_table(verify_data, [CONTENT_W*0.08, CONTENT_W*0.25, CONTENT_W*0.30, CONTENT_W*0.37]))
story.append(Spacer(1, 6))

# ━━ Chapter 6: License Data Format ━━
story.append(add_heading('<b>六、许可证数据格式</b>', h1_style, level=0))
story.append(Paragraph(
    '许可证数据采用JSON格式序列化，包含完整的授权信息。序列化后的JSON字符串使用RSA-2048私钥进行数字签名，'
    '签名和JSON数据组合为最终的许可证字符串。许可证格式为：Base64(JSON数据) + "." + Base64(RSA签名)。'
    '这种格式便于文本传输，同时通过RSA签名保证了数据的完整性和不可伪造性。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>6.1 LicenseData字段说明</b>', h2_style, level=1))

fields_data = [
    [Paragraph('<b>字段名</b>', header_cell_style), Paragraph('<b>类型</b>', header_cell_style),
     Paragraph('<b>说明</b>', header_cell_style)],
    [Paragraph('LicenseId', cell_style), Paragraph('String', cell_center_style),
     Paragraph('许可证唯一标识符（GUID）', cell_style)],
    [Paragraph('HardwareFingerprint', cell_style), Paragraph('String', cell_center_style),
     Paragraph('硬件指纹SHA-256哈希值（64字符十六进制）', cell_style)],
    [Paragraph('ProductName', cell_style), Paragraph('String', cell_center_style),
     Paragraph('产品名称', cell_style)],
    [Paragraph('ProductVersion', cell_style), Paragraph('String', cell_center_style),
     Paragraph('产品版本号', cell_style)],
    [Paragraph('CustomerName', cell_style), Paragraph('String', cell_center_style),
     Paragraph('客户名称', cell_style)],
    [Paragraph('LicenseType', cell_style), Paragraph('Enum', cell_center_style),
     Paragraph('许可证类型：Trial/Standard/Professional/Enterprise', cell_style)],
    [Paragraph('IssueDate', cell_style), Paragraph('String', cell_center_style),
     Paragraph('签发日期（ISO 8601格式）', cell_style)],
    [Paragraph('ExpiryDate', cell_style), Paragraph('String', cell_center_style),
     Paragraph('过期日期（空字符串表示永久授权）', cell_style)],
    [Paragraph('MaxActivationCount', cell_style), Paragraph('Integer', cell_center_style),
     Paragraph('最大激活次数', cell_style)],
    [Paragraph('Features', cell_style), Paragraph('String', cell_center_style),
     Paragraph('功能特性列表（逗号分隔）', cell_style)],
    [Paragraph('Remarks', cell_style), Paragraph('String', cell_center_style),
     Paragraph('备注信息', cell_style)],
]
story.append(Spacer(1, 6))
story.append(make_table(fields_data, [CONTENT_W*0.25, CONTENT_W*0.12, CONTENT_W*0.63]))
story.append(Spacer(1, 6))

# ━━ Chapter 7: Integration Guide ━━
story.append(add_heading('<b>七、集成指南</b>', h1_style, level=0))

story.append(Spacer(1, 12))
story.append(add_heading('<b>7.1 厂商端部署</b>', h2_style, level=1))
story.append(Paragraph(
    '厂商端需要完成以下部署步骤：首先，运行KeyGenerator工具生成RSA-2048密钥对，'
    '将私钥文件（private_key.xml）妥善保管在安全位置，切勿泄露或包含在客户端程序中；'
    '将公钥文件（public_key.xml）用于客户端程序集成。其次，如果需要在线授权功能，'
    '需要部署ASP.NET Web API服务，配置LicenseServerController，并设置RSA私钥和HMAC共享密钥。'
    '服务端应使用HTTPS协议，确保通信安全。最后，生成HMAC共享密钥，该密钥需要同时配置在客户端和服务端，'
    '用于在线请求的签名验证。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>7.2 客户端集成</b>', h2_style, level=1))
story.append(Paragraph(
    '客户端集成需要在WinForms程序中添加以下代码。首先，将RSA公钥XML字符串硬编码在程序中'
    '（建议使用混淆器增加逆向难度），然后在程序入口点（Main方法或主窗体Load事件）创建LicenseManager实例并执行验证。'
    '关键代码示例如下：', body_style))

story.append(code_block(
    "' 初始化授权管理器\n"
    "Dim licenseManager As New LicenseManager(\n"
    "    publicKeyXml:=GetEmbeddedPublicKey(),\n"
    "    productName:=\"我的商业软件\",\n"
    "    productVersion:=\"1.0.0\",\n"
    "    serverUrl:=\"https://license.example.com/api/activate\",\n"
    "    hmacKey:=GetEmbeddedHmacKey()\n"
    ")\n"
    "\n"
    "' 执行授权验证（先离线后在线）\n"
    "Dim result As LicenseValidationResult = licenseManager.Validate()\n"
    "\n"
    "' 验证失败时显示授权对话框\n"
    "If Not result.IsValid Then\n"
    "    Dim dlgResult As DialogResult = licenseManager.ValidateWithDialog(Me)\n"
    "    If Not licenseManager.IsLicensed Then\n"
    "        Application.Exit()\n"
    "    End If\n"
    "End If"
))

story.append(Spacer(1, 12))
story.append(add_heading('<b>7.3 项目引用配置</b>', h2_style, level=1))
story.append(Paragraph(
    '在VB.NET项目中集成本框架，需要添加以下引用和配置。首先，在项目中添加对System.Management的引用，'
    '用于WMI硬件信息采集。其次，添加对System.Web.Extensions的引用，用于JavaScriptSerializer进行JSON序列化。'
    '然后，将框架的所有.vb文件添加到项目中，按照Shared、Client、Vendor、Server目录结构组织。'
    '最后，确保项目的目标框架为.NET Framework 4.5或更高版本。', body_style))

refs_data = [
    [Paragraph('<b>引用名称</b>', header_cell_style), Paragraph('<b>用途</b>', header_cell_style)],
    [Paragraph('System.Management', cell_style), Paragraph('WMI硬件信息采集', cell_style)],
    [Paragraph('System.Web.Extensions', cell_style), Paragraph('JSON序列化/反序列化', cell_style)],
    [Paragraph('System.Net.Http', cell_style), Paragraph('在线授权HTTP通信', cell_style)],
    [Paragraph('System.Security', cell_style), Paragraph('RSA/AES/HMAC加密操作', cell_style)],
]
story.append(Spacer(1, 6))
story.append(make_table(refs_data, [CONTENT_W * 0.35, CONTENT_W * 0.65]))
story.append(Spacer(1, 6))

# ━━ Chapter 8: Security Best Practices ━━
story.append(add_heading('<b>八、安全最佳实践</b>', h1_style, level=0))

story.append(Spacer(1, 12))
story.append(add_heading('<b>8.1 密钥保护</b>', h2_style, level=1))
story.append(Paragraph(
    'RSA私钥是整个授权系统的核心安全资产，必须采取最严格的保护措施。私钥文件应存储在加密的离线存储介质上，'
    '仅在生成许可证时临时加载，使用完毕后立即从内存中清除。绝不能将私钥包含在客户端程序中，'
    '即使是混淆后的程序也可能被逆向工程提取密钥。建议使用硬件安全模块（HSM）存储私钥，'
    '或者在最低限度下使用操作系统的DPAPI（Data Protection API）对私钥文件进行加密存储。'
    '同时，应建立密钥轮换机制，定期更换RSA密钥对，并为不同产品或不同版本使用不同的密钥对。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>8.2 代码保护</b>', h2_style, level=1))
story.append(Paragraph(
    '.NET程序容易被反编译，因此必须对客户端程序进行代码保护。首先，使用专业的.NET混淆工具'
    '（如Dotfuscator、Crypto Obfuscator等）对程序集进行混淆处理，使反编译后的代码难以阅读和理解。'
    '其次，RSA公钥和HMAC密钥不应以明文形式存储在代码中，建议使用分散存储、动态拼接、'
    '或从加密资源中运行时解密等方式增加提取难度。再次，关键验证逻辑应增加反调试和反篡改检测，'
    '例如检测是否被附加调试器、验证程序集的数字签名等。最后，可以考虑将核心验证逻辑'
    '移植到非托管C++ DLL中，通过P/Invoke调用，增加逆向工程的难度。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>8.3 通信安全</b>', h2_style, level=1))
story.append(Paragraph(
    '在线授权的通信安全至关重要。生产环境必须使用HTTPS协议，且应配置强密码学套件，'
    '禁用TLS 1.0和TLS 1.1等不安全的协议版本。服务端应部署有效的SSL/TLS证书，'
    '客户端应验证服务器证书的有效性，防止中间人攻击。HMAC共享密钥应定期更换，'
    '且不同版本的客户端可以使用不同的HMAC密钥，通过API版本号区分。'
    '服务端应实现请求频率限制，防止暴力破解和拒绝服务攻击。'
    '同时，服务端应记录所有授权请求的日志，包括请求时间、硬件指纹、IP地址、验证结果等，'
    '便于安全审计和异常检测。', body_style))

story.append(Spacer(1, 12))
story.append(add_heading('<b>8.4 硬件变更容错</b>', h2_style, level=1))
story.append(Paragraph(
    '在实际使用中，客户可能会更换网卡、升级内存或更换硬盘，导致硬件指纹发生变化。'
    '为了平衡安全性和用户体验，本框架采用了多维硬件信息采集策略，不依赖单一硬件特征。'
    '当部分硬件发生变更时（如更换网卡），由于CPU ID和主板序列号等核心信息未变，'
    '硬件指纹仍然保持一致。如果核心硬件（CPU或主板）发生变更，则硬件指纹会改变，'
    '需要重新授权。厂商可以根据业务需求，在服务端实现灵活的硬件变更策略，'
    '例如允许一定次数的免费重新授权，或者对部分硬件变更进行自动迁移。', body_style))

# ━━ Chapter 9: File Structure ━━
story.append(add_heading('<b>九、项目文件结构</b>', h1_style, level=0))
story.append(Paragraph(
    '本框架的源代码按照职责划分为Shared（共享）、Client（客户端）、Vendor（厂商端）、Server（服务端）和Demo（示例）五个目录。'
    '开发者可以根据实际项目需求，选择性地集成所需的模块。客户端程序只需要Shared和Client目录下的文件；'
    '厂商端工具需要Shared和Vendor目录下的文件；服务端需要Shared和Server目录下的文件。', body_style))

story.append(code_block(
    "VBNetLicenseFramework/\n"
    "  Shared/\n"
    "    LicenseData.vb          ' 许可证数据模型（共享）\n"
    "    CryptoHelper.vb         ' 加密辅助工具（共享）\n"
    "  Client/\n"
    "    HardwareInfoCollector.vb ' 硬件信息采集\n"
    "    FingerprintGenerator.vb  ' 指纹哈希生成与脱敏\n"
    "    LicenseValidator.vb      ' 许可证验证\n"
    "    OfflineLicenseProvider.vb' 离线授权提供程序\n"
    "    OnlineLicenseProvider.vb ' 在线授权提供程序\n"
    "    LicenseManager.vb        ' 授权管理器（统一入口）\n"
    "    LicenseDialog.vb         ' 授权对话框UI\n"
    "  Vendor/\n"
    "    KeyGenerator.vb          ' RSA密钥对生成工具\n"
    "    LicenseGenerator.vb      ' 许可证生成模块\n"
    "    LicenseGeneratorTool.vb  ' 许可证生成WinForms工具\n"
    "  Server/\n"
    "    LicenseServerController.vb' ASP.NET在线授权API控制器\n"
    "  Demo/\n"
    "    MainForm.vb              ' WinForms集成示例"
))

# ━━ Build ━━
doc.multiBuild(story)
print(f"PDF generated: {output_path}")
